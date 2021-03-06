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
using Smart.FA.Catalog.Web.Authentication.Header.Validators;
using AccountHeadersValidator = Smart.FA.Catalog.Web.Authentication.Header.Validators.AccountHeadersValidator;

namespace Smart.FA.Catalog.Web.Authentication.Handlers;

/// <summary>
/// The authentication is working with a system from NGINX (the proxy in front of our apps) called x-accel (https://www.nginx.com/resources/wiki/start/topics/examples/x-accel/).
/// The proxy header middleware parses incoming request from NGINX to fetch a user and the name of the application the call originated.
/// It will then proceed to log the user as a trainer for the rest of the request lifetime.
/// </summary>
public class UserAdminAuthenticationHandler : AuthenticationHandler<CfaAuthenticationOptions>
{
    private readonly AccountHeadersValidator _accountHeadersValidator;
    private readonly CustomDataFieldsValidator _customDataFieldsDataValidator;
    private readonly IMediator _mediator;
    private readonly IAccountDataHeaderSerializer _accountDataHeaderSerializer;
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
        IAccountDataHeaderSerializer accountDataHeaderSerializer,
        ISystemClock clock) : base(options, logger, encoder, clock)
    {
        _accountHeadersValidator = new AccountHeadersValidator();
        _customDataFieldsDataValidator = new CustomDataFieldsValidator();
        _mediator = mediator;
        _accountDataHeaderSerializer = accountDataHeaderSerializer;
        _authenticationOptions = authenticationOptions.CurrentValue;
    }

    /// <inheritdoc />
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        try
        {
            // Check if the headers contains the expected values set during Account redirection.
            // An exception will be thrown if any of the headers are invalid.
            EnsureHeaders();

            // Retrieve the trainer profile by its smart id.
            var currentTrainer = await GetTrainerBySmartUserIdAndApplicationTypeAsync();

            // Handle first time a Smart user connects in FA.
            if (currentTrainer is null)
            {
                Logger.LogInformation($"User `{_userId}` connected from `{_appName}` for the first time in FA. Creating a trainer for the user.");
                currentTrainer = await CreateTrainerAsync();
            }

            // Set up trainer's identity that will be used during requests.
            await SetUserIdentityAsync(currentTrainer);

            var ticket = new AuthenticationTicket(Context.User, Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }
        catch (AccountHeadersMissingException e)
        {
            return AuthenticateResult.Fail(e);
        }
        catch (Exception exception)
        {
            Logger.LogCritical(exception, "An error occurred while authenticating");
            return AuthenticateResult.Fail(new Exception("An issue occurred during authentication"));
        }
    }

    /// <summary>
    /// Checks if the required headers that should be set by Account are actually set.
    /// </summary>
    /// <exception cref="AccountHeadersMissingException">Occurs when there is one or more required headers missing.</exception>
    private void EnsureHeaders()
    {
        SetFakeHeaderValueIfOptionSetToTrue();

        ThrowIfHeadersAreInvalid();

        _userId = Context.Request.Headers[Headers.UserId].ToString();
        _appName = Context.Request.Headers[Headers.ApplicationName].ToString();

        var accountDataString = Context.Request.Headers[Headers.AccountData];
        var accountData = _accountDataHeaderSerializer.Deserialize(accountDataString);

        ThrowIfCustomDataInvalid(accountData);

        _firstName = accountData!.FirstName!;
        _lastName = accountData.LastName!;
        _email = accountData.Email!;
    }

    /// <summary>
    /// Sets hardcoded headers values if the UseFakeHeaders option is enabled.
    /// </summary>
    private void SetFakeHeaderValueIfOptionSetToTrue()
    {
        // A user will then bypass any NGNIX redirection (usefull for debugging locally for example)
        if (_authenticationOptions.UseFakeHeaders)
        {
            Context.Request.Headers.Add(Headers.UserId, "1");
            Context.Request.Headers.Add(Headers.ApplicationName, ApplicationType.Account.Name);
            Context.Request.Headers.Add(Headers.AccountData, _accountDataHeaderSerializer.CreateFakeAccountDataHeader());
        }
    }

    private void ThrowIfHeadersAreInvalid()
    {
        var accountValidationFailures = _accountHeadersValidator.Validate(Context.Request.Headers);

        // We log as critical is any header is missing.
        if (accountValidationFailures.Any())
        {
            Logger.LogCritical(string.Join(", ", accountValidationFailures));
            throw new AccountHeadersMissingException("One more required header was not set by Account during the redirection");
        }
    }

    private void ThrowIfCustomDataInvalid(AccountData? accountData)
    {
        var accountCustomDataValidationFailures = _customDataFieldsDataValidator.Validate(accountData);

        // We log as critical is any data field in the customData header is missing.
        if (accountCustomDataValidationFailures.Any())
        {
            Logger.LogCritical(string.Join(", ", accountCustomDataValidationFailures));
            throw new AccountHeadersMissingException("One more required fields in the customData header was not set by Account during the redirection");
        }
    }

    private async Task<Trainer?> GetTrainerBySmartUserIdAndApplicationTypeAsync()
    {
        return (await _mediator.Send(new GetTrainerByUserAppRequest(applicationType: ApplicationType.FromName(_appName!), userId: _userId!))).Trainer;
    }

    private async Task<Trainer> CreateTrainerAsync()
    {
        var createTrainerRequest = new CreateTrainerFromUserAppRequest() { User = new UserDto(_userId!, _firstName!, _lastName!, _appName!, _email!) };
        var createdTrainerResponse = await _mediator.Send(createTrainerRequest);

        return createdTrainerResponse.Trainer;
    }

    private async Task SetUserIdentityAsync(Trainer trainer)
    {
        // Sets the data for the IUserIdentity service.
        var isSuperUser = await _mediator.Send(new IsSuperUserQuery(trainer.Id));
        Context.User = new GenericPrincipal(new CustomIdentity(trainer), roles: isSuperUser ? new[] { "SuperUser" } : null);

        // Updates the first name, last name and email address of the current trainer if they changed for any reason.
        // If anything goes wrong an exception will be thrown and stops execution of the HTTP request.
        await _mediator.Send(new UpdateTrainerIdentityCommand(trainer.Id, _firstName!, _lastName!, _email!));
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
