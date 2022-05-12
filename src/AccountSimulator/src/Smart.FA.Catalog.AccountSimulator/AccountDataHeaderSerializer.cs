using System.Text.Json;

namespace Smart.FA.Catalog.AccountSimulator;

public class AccountDataHeaderSerializer : IAccountDataHeaderSerializer
{
    private AccountData CreateMock(string firstName, string lastName, string email)
    {
        return new AccountData { FirstName = firstName, LastName = lastName, Email = email };
    }

    public string Serialize( AccountData accountData)
    {
        var serializedCustomData = JsonSerializer.Serialize(accountData);
        return System.Web.HttpUtility.UrlDecode(serializedCustomData);
    }

    public AccountData? Deserialize(string accountDataSerialized)
    {
        if (string.IsNullOrEmpty(accountDataSerialized))
        {
            return new AccountData();
        }

        return JsonSerializer.Deserialize<AccountData?>(accountDataSerialized);
    }

    public AccountData GetByUserId(string userId)
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
