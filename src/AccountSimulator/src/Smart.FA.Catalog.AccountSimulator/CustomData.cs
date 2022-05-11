using Newtonsoft.Json;

namespace Smart.FA.Catalog.AccountSimulator;

public class CustomData
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
}

public static class CustomDataFactory
{
    private static CustomData CreateMock(string firstName, string lastName, string email)
    {
        return new CustomData { FirstName = firstName, LastName = lastName, Email = email };
    }

    public static string Serialize(this CustomData customData)
    {
        var serializedCustomData = JsonConvert.SerializeObject(customData);
        return System.Web.HttpUtility.UrlDecode(serializedCustomData);
    }

    public static CustomData Deserialize(string serializedCustomData)
    {
        if (string.IsNullOrEmpty(serializedCustomData))
        {
            return new CustomData();
        }

        return JsonConvert.DeserializeObject<CustomData>(serializedCustomData);
    }

    public static CustomData GetByUserId(string userId)
    {
        switch (userId)
        {
            case "1": return CreateMock("Victor", "vD", "victor@victor.com");
            case "2":
            default:
                return CreateMock("Maxime", "P.", "maxime@maxime.com");
        }
    }
}
