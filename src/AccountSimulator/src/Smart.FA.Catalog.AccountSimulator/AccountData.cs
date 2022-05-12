using System.Text.Json;

namespace Smart.FA.Catalog.AccountSimulator;

public class AccountData
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
}

public static class AccountDataFactory
{
    private static AccountData CreateMock(string firstName, string lastName, string email)
    {
        return new AccountData { FirstName = firstName, LastName = lastName, Email = email };
    }

    public static string Serialize(this AccountData accountData)
    {
        var serializedCustomData = JsonSerializer.Serialize(accountData);
        return System.Web.HttpUtility.UrlDecode(serializedCustomData);
    }

    public static AccountData? Deserialize(string serializedCustomData)
    {
        if (string.IsNullOrEmpty(serializedCustomData))
        {
            return new AccountData();
        }

        return JsonSerializer.Deserialize<AccountData?>(serializedCustomData);
    }

    public static AccountData GetByUserId(string userId)
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
