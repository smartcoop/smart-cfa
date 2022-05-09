using System.Security.Principal;
using System.Text.Encodings.Web;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Smart.FA.Catalog.Application.UseCases.Commands;
using Smart.FA.Catalog.Application.UseCases.Queries;
using Smart.FA.Catalog.Application.UseCases.Queries.Authorization;
using Smart.FA.Catalog.Core.Domain;
using Smart.FA.Catalog.Core.Domain.Models;
using Smart.FA.Catalog.Core.Domain.User.Enumerations;

namespace Smart.FA.Catalog.Web.Authentication.Handlers;

/// <summary>
/// The authentication is working with a system from Nginx (the proxy in front of our apps) called x-accel (https://www.nginx.com/resources/wiki/start/topics/examples/x-accel/).
/// The proxy header middleware parse incoming request from nginx to fetch a user and the name of the application the call originated.
/// It will then proceed to log the user as a trainer for the rest of the request lifetime.
/// </summary>
public class UserAdminAuthenticationHandler : AuthenticationHandler<CfaAuthenticationOptions>
{
    private readonly IMediator _mediator;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private string? _userId;
    private string? _appName;

    public UserAdminAuthenticationHandler(
        IMediator mediator,
        IWebHostEnvironment webHostEnvironment,
        IOptionsMonitor<CfaAuthenticationOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock) : base(options, logger, encoder, clock)
    {
        _mediator = mediator;
        _webHostEnvironment = webHostEnvironment;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        try
        {
            // Let's check if the header contains the expected headers set during Account redirection.
            // An exception will be thrown if the headers are invalid.
            EnsureHeaders();

            var response = await GetTrainerBySmartUserIdAndApplicationTypeAsync();

            if (response.Trainer is null)
            {
                Logger.LogInformation($"User `{_userId}` connected from `{_appName}` for the first in FA. Creating a trainer for the user.");
                await CreateTrainerAsync(response);
            }

            await SetUserIdentityAsync(response.Trainer!);

            var ticket = new AuthenticationTicket(Context.User, Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }
        catch(Exception exception)
        {
            if (exception is not AccountHeadersMissingException)
            {
                Logger.LogCritical(exception, "An error occurred while authenticating");
            }

            return AuthenticateResult.Fail(new Exception("An issue occurred during authentication"));
        }
    }

    /// <summary>
    /// Checks if the required headers that should be set by Account are actually set.
    /// </summary>
    /// <exception cref="AccountHeadersMissingException">Occurs when there is one or more required headers missing.</exception>
    private void EnsureHeaders()
    {
        SetFakeHeaderValueIfDevelopmentEnvironmentAndMissing();

        ThrowIfHeadersInvalid();

        _userId = Context.Request.Headers["userid"].ToString();
        _appName = Context.Request.Headers["smartApplication"].ToString();
    }

    private void ThrowIfHeadersInvalid()
    {
        var errorMessage = new List<string>();

        if (!Context.Request.Headers.ContainsKey("userid"))
        {
            errorMessage.Add($"No userid header not found");
        }

        if (!Context.Request.Headers.ContainsKey("smartApplication"))
        {
            errorMessage.Add("smartApplication header not found");
        }

        // We log as critical is any header is missing.
        if (errorMessage.Any())
        {
            Logger.LogCritical(string.Join(", ", errorMessage));
            throw new AccountHeadersMissingException("One more required header was not set by Account during the redirection");
        }
    }

    private async Task<GetTrainerFromUserAppResponse> GetTrainerBySmartUserIdAndApplicationTypeAsync()
    {
        return await _mediator.Send(new GetTrainerFromUserAppRequest { UserId = _userId!, ApplicationType = ApplicationType.FromName(_appName!) });
    }

    private async Task CreateTrainerAsync(GetTrainerFromUserAppResponse response)
    {
        var newTrainerResponse = await _mediator.Send(new CreateTrainerFromUserAppRequest { User = response.User });
        response.Trainer = newTrainerResponse.Trainer ??
                           throw new InvalidOperationException($"{nameof(CreateTrainerAsync)} no trainer was returned from the creation");
    }

    private async Task SetUserIdentityAsync(Trainer trainer)
    {
        var isAdmin = await _mediator.Send(new IsSuperUserQuery(trainer.Id));
        Context.User = new GenericPrincipal(new CustomIdentity(trainer), roles: isAdmin ? new[] { "SuperUser" } : null);
    }

    private void SetFakeHeaderValueIfDevelopmentEnvironmentAndMissing()
    {
        // During local development the developer may not pass through ngnix redirection therefore, default values are set for him/her.
        if (_webHostEnvironment.IsDevelopment() || _webHostEnvironment.IsLocalEnvironment())
        {
            Context.Request.Headers.Add("userid", "1");
            Context.Request.Headers.Add("smartApplication", ApplicationType.Account.Name);
        }
    }
}

public class CfaAuthenticationOptions
    : AuthenticationSchemeOptions
{
}

public class AccountHeadersMissingException : Exception
{
    public AccountHeadersMissingException(string? message) : base(message)
    {
    }
}
