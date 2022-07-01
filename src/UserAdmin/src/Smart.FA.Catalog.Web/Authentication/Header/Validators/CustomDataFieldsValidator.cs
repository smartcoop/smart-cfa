namespace Smart.FA.Catalog.Web.Authentication.Header.Validators;

/// <summary>
/// Validator for the customData header holding the current user information.
/// </summary>
internal class CustomDataFieldsValidator
{
    /// <summary>
    /// Validates the customData header holding the current user information.
    /// </summary>
    public List<string> Validate(AccountData? accountData)
    {
        var validationFailures = new List<string>();

        if (accountData is null)
        {
            validationFailures.Add($"{nameof(AccountData)} not found in {Headers.AccountData} header");

            return validationFailures;
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

        return validationFailures;
    }
}
