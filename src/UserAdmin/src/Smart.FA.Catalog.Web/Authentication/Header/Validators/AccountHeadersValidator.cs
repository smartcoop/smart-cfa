namespace Smart.FA.Catalog.Web.Authentication.Header.Validators;

/// <summary>
/// Validator for the presence of headers set by Account.
/// </summary>
internal class AccountHeadersValidator
{
    /// <summary>
    /// Validates the presence of headers set by Account.
    /// </summary>
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
