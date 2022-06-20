using System.Security.Principal;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
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
using Smart.FA.Catalog.Core.Domain.ValueObjects;
using Smart.FA.Catalog.Shared.Extensions;
using Smart.FA.Catalog.Web.Options;
using Smart.FA.Catalog.Web.Authentication.Header;
using Smart.FA.Catalog.Web.Authorization.Role;

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
    private readonly IAccountDataHeaderSerializer _accountDataHeaderSerializer;
    private string _userId = string.Empty;
    private string _appName = string.Empty;
    private string _firstName = string.Empty;
    private string _lastName = string.Empty;
    private string _email = string.Empty;
    private AdminBehindUser? _adminBehindUser;
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
        SetFakeHeaderValueIfOptionSetToTrue();

        ThrowIfHeadersInvalid();

        _userId = Context.Request.Headers[Headers.UserId].ToString();
        _appName = Context.Request.Headers[Headers.ApplicationName].ToString();

        var accountDataString = Context.Request.Headers[Headers.AccountData];
        var accountData = _accountDataHeaderSerializer.Deserialize(accountDataString);

        ThrowIfCustomDataInvalid(accountData);

        _firstName = accountData!.FirstName!;
        _lastName = accountData.LastName!;
        _email = accountData.Email!;
        _adminBehindUser = accountData.AdminBehindUser;
    }

    private void SetFakeHeaderValueIfOptionSetToTrue()
    {
        // If the UserFakeHeaders option is set to true, the developer may not pass through ngnix redirection therefore, default values are set for him/her.
        if (_authenticationOptions.UseFakeHeaders)
        {
            Context.Request.Headers.Add(Headers.UserId, "1");
            Context.Request.Headers.Add(Headers.ApplicationName, ApplicationType.Account.Name);
            Context.Request.Headers.Add(Headers.AccountData, _accountDataHeaderSerializer.CreateSerializedMock());
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
        var roles = new List<string>();
        roles.AddIf(() => isAdmin, Roles.SuperUser);
        roles.AddIf(() => IsSocialMember(trainer.Identity.UserId), Roles.SocialMember);
        var connectedUser = _adminBehindUser is null
            ? null
            : new ConnectedUser(_adminBehindUser.UserId!, _adminBehindUser.Email!, Name.Create(_adminBehindUser.FirstName!, _adminBehindUser.LastName!).Value);
        Context.User = new GenericPrincipal(new CustomIdentity(trainer, connectedUser), roles.ToArray());
        // Updates the first name, last name and email address of the current trainer if they changed for any reason.
        // If anything goes wrong an exception will be thrown and stops execution of the HTTP request.
        await _mediator.Send(new UpdateTrainerIdentityCommand(trainer.Id, _firstName, _lastName, _email));
    }

    /// <summary>
    /// Check if the connected user is not impersonating a social member and is directly connected as a Permanent member.
    /// The "AD" prefix before the user id is a convention adopted between the app to indicate permanent member access.
    /// </summary>
    /// <param name="userId">The user id of the connected user</param>
    /// <returns></returns>
    private static bool IsSocialMember(string userId) => !Regex.Match(userId, "^AD").Success;
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
