using System.Text.Json;
using Smart.FA.Catalog.Core.Extensions;

namespace Smart.FA.Catalog.Web.Authentication.Header;

// The extra data passed in the header "customData" received by Nginx
public class AccountData
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
}

public static class AccountDataFactory
{
    private static AccountData CreateMock()
    {
        return new AccountData { FirstName = "Maxime", LastName = "P.", Email = "maxime@maxime.com" };
    }

    public static string CreateSerializedMock()
    {
        var customData = CreateMock();
        var serializedCustomData = customData.ToJson();
        return System.Web.HttpUtility.UrlDecode(serializedCustomData);
    }

    public static AccountData? Deserialize(string serializedCustomData)
    {
        return serializedCustomData.FromJson<AccountData>();
    }
}
