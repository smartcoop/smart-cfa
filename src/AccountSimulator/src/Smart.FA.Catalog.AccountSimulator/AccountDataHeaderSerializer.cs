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
        return userId switch
        {
            "1" => CreateMock("Victor", "vD", "victor@victor.com"),
            "2" => CreateMock("Maxime", "P.", "maxime@maxime.com"),
            _ => throw new InvalidDataException("User id is invalid")
        };
    }
}
