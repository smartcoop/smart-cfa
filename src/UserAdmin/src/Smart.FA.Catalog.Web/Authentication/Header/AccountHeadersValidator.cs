namespace Smart.FA.Catalog.Web.Authentication.Header;

internal class AccountHeadersValidator
{
    public List<string> Validate(IHeaderDictionary headerDictionary)
    {
        var validationFailures = new List<string>();

        if (!headerDictionary.ContainsKey(Headers.UserId))
        {
            validationFailures.Add($"{Headers.UserId} header not found");
        }

        if (!headerDictionary.ContainsKey(Headers.ApplicationName))
        {
            validationFailures.Add($"{Headers.ApplicationName} header not found");
        }

        if (!headerDictionary.ContainsKey(Headers.AccountData))
        {
            validationFailures.Add($"{Headers.AccountData} header not found");
        }

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
        }

        if (accountData.FirstName is null)
        {
            validationFailures.Add($"{nameof(accountData.FirstName)} field not found in {Headers.AccountData} header");
        }

        if (accountData.LastName is null)
        {
            validationFailures.Add($"{nameof(accountData.LastName)} field not found in {Headers.AccountData} header");
        }

        if (accountData.Email is null)
        {
            validationFailures.Add($"{nameof(accountData.Email)} field not found in {Headers.AccountData} header");
        }

        if (accountData.AdminBehindUser is not null)
        {
            if (accountData.AdminBehindUser!.FirstName is null)
            {
                validationFailures.Add($"Admin - {nameof(accountData.AdminBehindUser.FirstName)} field not found in {Headers.AccountData} header");
            }

            if (accountData.AdminBehindUser!.LastName is null)
            {
                validationFailures.Add($"Admin - {nameof(accountData.AdminBehindUser.LastName)} field not found in {Headers.AccountData} header");
            }

            if (accountData.AdminBehindUser!.Email is null)
            {
                validationFailures.Add($"Admin - {nameof(accountData.AdminBehindUser.Email)} field not found in {Headers.AccountData} header");
            }

            if (accountData.AdminBehindUser!.UserId is null)
            {
                validationFailures.Add($"Admin - {nameof(accountData.AdminBehindUser.UserId)} field not found in {Headers.AccountData} header");
            }
        }

        return validationFailures;
    }
}
