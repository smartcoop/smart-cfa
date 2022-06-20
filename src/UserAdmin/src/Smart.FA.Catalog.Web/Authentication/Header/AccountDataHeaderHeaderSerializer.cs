using Smart.FA.Catalog.Core.Extensions;

namespace Smart.FA.Catalog.Web.Authentication.Header;

public class AccountDataHeaderHeaderSerializer : IAccountDataHeaderSerializer
{
    private AccountData CreateMock()
    {
        return new AccountData
        {
            FirstName = "Maxime",
            LastName = "P.",
            Email = "maxime@maxime.com",
            AdminBehindUser = new AdminBehindUser { Email = "victor@victor.com", FirstName = "Victor", LastName = "v.", UserId = "AD-1001" }
        };
    }

    /// <inheritdoc />
    public string CreateSerializedMock()
    {
        var customData = CreateMock();
        var serializedCustomData = customData.ToJson();
        return System.Web.HttpUtility.UrlDecode(serializedCustomData);
    }

    /// <inheritdoc />
    public AccountData? Deserialize(string serializedCustomData)
    {
        return serializedCustomData.FromJson<AccountData>();
    }
}
