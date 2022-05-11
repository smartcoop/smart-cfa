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
using Smart.FA.Catalog.Core.Domain.User.Dto;
using Smart.FA.Catalog.Core.Domain.User.Enumerations;
using Smart.FA.Catalog.Web.Options;
using Smart.FA.Catalog.Web.Authentication.Header;

namespace Smart.FA.Catalog.Web.Authentication.Handlers;

/// <summary>
/// The authentication is working with a system from Nginx (the proxy in front of our apps) called x-accel (https://www.nginx.com/resources/wiki/start/topics/examples/x-accel/).
/// The proxy header middleware parse incoming request from nginx to fetch a user and the name of the application the call originated.
/// It will then proceed to log the user as a trainer for the rest of the request lifetime.
/// </summary>
public class UserAdminAuthenticationHandler : AuthenticationHandler<CfaAuthenticationOptions>
{
    private readonly AccountHeadersValidator _accountHeadersValidator;
    private readonly CustomDataFieldsValidator _customDataFieldsDataValidator;
    private readonly IMediator _mediator;
    private string? _userId;
    private string? _appName;
    private string? _firstName;
    private string? _lastName;
    private string? _email;
    private readonly SpecialAuthenticationOptions _authenticationOptions;

    public UserAdminAuthenticationHandler(
        IMediator mediator,
        IOptionsMonitor<CfaAuthenticationOptions> options,
        IOptionsMonitor<SpecialAuthenticationOptions> authenticationOptions,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock) : base(options, logger, encoder, clock)
    {
        _accountHeadersValidator = new AccountHeadersValidator();
        _customDataFieldsDataValidator = new CustomDataFieldsValidator();
        _mediator = mediator;
        _authenticationOptions = authenticationOptions.CurrentValue;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        try
        {
            // Checks if the headers contain the expected values set during Account redirection.
            // An exception will be thrown if any of the headers are invalid.
            EnsureHeaders();

            // Retrieves the trainer profile by its smart id.
            var currentTrainer = await GetTrainerBySmartUserIdAndApplicationTypeAsync();

            // First time a Smart user connects in FA.
            if (currentTrainer is null)
            {
                Logger.LogInformation($"User `{_userId}` connected from `{_appName}` for the first time in FA. Creating a trainer for the user.");
                currentTrainer = await CreateTrainerAsync();
            }

            // Set up trainer's identity that will be used across the application.
            await SetUserIdentityAsync(currentTrainer);

            var ticket = new AuthenticationTicket(Context.User, Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }
        catch (Exception exception)
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

        _userId = Context.Request.Headers[Headers.UserId].ToString();
        _appName = Context.Request.Headers[Headers.ApplicationName].ToString();

        var customDataString = Context.Request.Headers[Headers.CustomData];
        var customData = CustomDataFactory.Deserialize(customDataString);

        ThrowIfCustomDataInvalid(customData);

        _firstName = customData.FirstName!;
        _lastName = customData.LastName!;
        _email = customData.Email!;
    }

    private void SetFakeHeaderValueIfDevelopmentEnvironmentAndMissing()
    {
        // If the UserFakeHeaders option is set to true, the developer may not pass through ngnix redirection therefore, default values are set for him/her.
        if (_authenticationOptions.UseFakeHeaders)
        {
            Context.Request.Headers.Add(Headers.UserId, "1");
            Context.Request.Headers.Add(Headers.ApplicationName, ApplicationType.Account.Name);
            Context.Request.Headers.Add(Headers.CustomData, CustomDataFactory.CreateSerializedMock());
        }
    }

    private void ThrowIfHeadersInvalid()
    {
        var accountValidationFailures = _accountHeadersValidator.Validate(Context.Request.Headers);

        // We log as critical is any header is missing.
        if (accountValidationFailures.Any())
        {
            Logger.LogCritical(string.Join(", ", accountValidationFailures));
            throw new AccountHeadersMissingException("One more required header was not set by Account during the redirection");
        }
    }

    private void ThrowIfCustomDataInvalid(CustomData customData)
    {
        var accountCustomDataValidationFailures = _customDataFieldsDataValidator.Validate(customData);

        // We log as critical is any data field in the customData header is missing.
        if (accountCustomDataValidationFailures.Any())
        {
            Logger.LogCritical(string.Join(", ", accountCustomDataValidationFailures));
            throw new AccountHeadersMissingException("One more required fields in the customData header was not set by Account during the redirection");
        }
    }

    private async Task<Trainer?> GetTrainerBySmartUserIdAndApplicationTypeAsync()
    {
        return (await _mediator.Send(new GetTrainerFromUserAppRequest(applicationType: ApplicationType.FromName(_appName), userId: _userId))).Trainer;
    }

    private async Task<Trainer> CreateTrainerAsync()
    {
        var createTrainerRequest = new CreateTrainerFromUserAppRequest() { User = new UserDto(_userId, _firstName, _lastName, _appName, _email) };
        var createdTrainerResponse = await _mediator.Send(createTrainerRequest);

        return createdTrainerResponse.Trainer;
    }

    private async Task SetUserIdentityAsync(Trainer trainer)
    {
        // Sets the data for the IUserIdentity service.
        var isAdmin = await _mediator.Send(new IsSuperUserQuery(trainer.Id));
        Context.User = new GenericPrincipal(new CustomIdentity(trainer), roles: isAdmin ? new[] { "SuperUser" } : null);

        // Updates the first name, last name and email address of the current trainer if they changed for any reason.
        // If anything goes wrong an exception will be thrown and stops execution of the HTTP request.
        await _mediator.Send(new UpdateTrainerIdentityCommand(trainer.Id, _firstName, _lastName, _email));
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
