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

        if (!headerDictionary.ContainsKey(Headers.CustomData))
        {
            validationFailures.Add($"{Headers.CustomData} header not found");
        }

        return validationFailures;
    }
}

internal class CustomDataFieldsValidator
{
    public List<string> Validate(CustomData customData)
    {
        var validationFailures = new List<string>();

        if (customData.FirstName is null)
        {
            validationFailures.Add($"{nameof(customData.FirstName)} field not found in {Headers.CustomData} header");
        }

        if (customData.LastName is null)
        {
            validationFailures.Add($"{nameof(customData.LastName)} field not found in {Headers.CustomData} header");
        }

        if (customData.Email is null)
        {
            validationFailures.Add($"{nameof(customData.Email)} field not found in {Headers.CustomData} header");
        }

        return validationFailures;
    }
}
