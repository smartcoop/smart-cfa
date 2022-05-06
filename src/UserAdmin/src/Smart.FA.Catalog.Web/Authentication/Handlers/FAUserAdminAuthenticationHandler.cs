using System.Security.Claims;
using System.Security.Principal;
using System.Text.Encodings.Web;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Smart.FA.Catalog.Application.UseCases.Commands;
using Smart.FA.Catalog.Application.UseCases.Queries;
using Smart.FA.Catalog.Application.UseCases.Queries.Authorization;
using Smart.FA.Catalog.Core.Domain.Models;
using Smart.FA.Catalog.Core.Domain.User.Enumerations;
using Smart.FA.Catalog.Shared.Domain.Enumerations;

namespace Smart.FA.Catalog.Web.Authentication.Handlers;

/// <summary>
/// The authentication is working with a system from Nginx (the proxy in front of our apps) called x-accel (https://www.nginx.com/resources/wiki/start/topics/examples/x-accel/).
/// The proxy header middleware parse incoming request from nginx to fetch a user and the name of the application the call originated.
/// It will then proceed to log the user as a trainer for the rest of the request lifetime.
/// </summary>
public class FAUserAdminAuthenticationHandler : AuthenticationHandler<CfaAuthenticationOptions>
{
    private readonly IMediator _mediator;

    public FAUserAdminAuthenticationHandler(
        IMediator mediator,
        IOptionsMonitor<CfaAuthenticationOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock) : base(options, logger, encoder, clock)
    {
        _mediator = mediator;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        try
        {
            //Default values are used to substitute to the Nginx authentication system
            //TODO: When the nginx authentication system is in place in every environment, remove default values
            var response = await GetTrainerBySmartUserIdAndApplicationTypeAsync();

            if (response.Trainer is null)
            {
                await CreateTrainerAsync(response);
            }

            await SetUserIdentityAsync(response);

            var ticket = new AuthenticationTicket(Context.User, Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }
        catch
        {
            return AuthenticateResult.Fail(new Exception("An issue occurred during authentication"));
        }
    }

    private async Task<GetTrainerFromUserAppResponse> GetTrainerBySmartUserIdAndApplicationTypeAsync()
    {
        var userId          = string.IsNullOrEmpty(Context.Request.Headers["userid"].ToString()) ? "1" : Context.Request.Headers["userid"].ToString();
        var appName         = string.IsNullOrEmpty(Context.Request.Headers["smartApplication"].ToString()) ? ApplicationType.Account.Name : Context.Request.Headers["smartApplication"].ToString();
        var applicationType = Enumeration.FromDisplayName<ApplicationType>(appName);

        var response = await _mediator.Send(new GetTrainerFromUserAppRequest { UserId = userId, ApplicationType = applicationType });
        return response;
    }

    private async Task CreateTrainerAsync(GetTrainerFromUserAppResponse response)
    {
        var newTrainerResponse = await _mediator.Send(new CreateTrainerFromUserAppRequest { User = response.User });
        response.Trainer = newTrainerResponse.Trainer;
    }

    private async Task SetUserIdentityAsync(GetTrainerFromUserAppResponse response)
    {
        var isAdmin = await _mediator.Send(new IsSuperUserQuery(response.User.UserId));
        Context.User = new GenericPrincipal(new CustomIdentity(response.Trainer!), roles: isAdmin ? new[] { "SuperUser" } : null);
    }
}

public class CfaAuthenticationOptions
    : AuthenticationSchemeOptions
{
}
