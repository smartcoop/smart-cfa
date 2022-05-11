using System.Text.Json.Nodes;
using Newtonsoft.Json;

namespace Smart.FA.Catalog.Web.Authentication.Header;

public class CustomData
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
}

public static class CustomDataFactory
{
    private static CustomData CreateMock()
    {
        return new CustomData { FirstName = "Maximé", LastName = "P.", Email = "maxime@maxime.com" };
    }

    public static string CreateSerializedMock()
    {
        var customData = CreateMock();
        var serializedCustomData = JsonConvert.SerializeObject(customData);
        return System.Web.HttpUtility.UrlDecode(serializedCustomData);
    }

    public static CustomData Deserialize(string serializedCustomData)
    {
        return JsonConvert.DeserializeObject<CustomData>(serializedCustomData);
    }
}


