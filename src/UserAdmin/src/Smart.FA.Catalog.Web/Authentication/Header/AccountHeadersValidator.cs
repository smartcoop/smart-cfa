using Smart.FA.Catalog.Shared.Extensions;

namespace Smart.FA.Catalog.Web.Authentication.Header;

internal class AccountHeadersValidator
{
    public List<string> Validate(IHeaderDictionary headerDictionary)
    {
        var validationFailures = new List<string>();
        validationFailures.AddIf(() => !headerDictionary.ContainsKey(Headers.UserId), $"{Headers.UserId} header not found");
        validationFailures.AddIf(() => !headerDictionary.ContainsKey(Headers.ApplicationName), $"{Headers.ApplicationName} header not found");
        validationFailures.AddIf(() => !headerDictionary.ContainsKey(Headers.AccountData), $"{Headers.AccountData} header not found");
        return validationFailures;
    }
}

internal class CustomDataFieldsValidator
{
    public List<string> Validate(AccountData? accountData)
    {
        var validationFailures = new List<string>();

        if (accountData is null)
        {
            validationFailures.Add($"{nameof(AccountData)} not found in {Headers.AccountData} header");
            return validationFailures;
        }

        validationFailures.AddIf(() => accountData.FirstName is null, $"{nameof(accountData.FirstName)} field not found in {Headers.AccountData} header");
        validationFailures.AddIf(() => accountData.LastName is null, $"{nameof(accountData.LastName)} field not found in {Headers.AccountData} header");
        validationFailures.AddIf(() => accountData.Email is null, $"{nameof(accountData.Email)} field not found in {Headers.AccountData} header");

        if (accountData.AdminBehindUser is null)
        {
            return validationFailures;
        }

        validationFailures.AddIf(() => accountData.AdminBehindUser.FirstName is null, $"Admin - {nameof(accountData.AdminBehindUser.FirstName)} field not found in {Headers.AccountData} header");
        validationFailures.AddIf(() => accountData.AdminBehindUser.LastName is null, $"Admin - {nameof(accountData.AdminBehindUser.LastName)} field not found in {Headers.AccountData} header");
        validationFailures.AddIf(() => accountData.AdminBehindUser.Email is null, $"Admin - {nameof(accountData.AdminBehindUser.Email)} field not found in {Headers.AccountData} header");
        validationFailures.AddIf(() => accountData.AdminBehindUser.UserId is null, $"Admin - {nameof(accountData.AdminBehindUser.UserId)} field not found in {Headers.AccountData} header");

        return validationFailures;
    }
}
