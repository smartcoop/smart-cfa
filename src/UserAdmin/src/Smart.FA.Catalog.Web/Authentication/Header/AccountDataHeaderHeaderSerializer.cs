using Smart.FA.Catalog.Core.Extensions;

namespace Smart.FA.Catalog.Web.Authentication.Header;

public class AccountDataHeaderHeaderSerializer : IAccountDataHeaderSerializer
{
    private AccountData CreateFakeCustomData()
    {
        return new AccountData { FirstName = "John", LastName = "Doe", Email = "john.doe@unknown.com" };
    }

    /// <inheritdoc />
    public string CreateFakeAccountDataHeader()
    {
        var customData = CreateFakeCustomData();
        var serializedCustomData = customData.ToJson();
        return System.Web.HttpUtility.UrlDecode(serializedCustomData);
    }

    /// <inheritdoc />
    public AccountData? Deserialize(string serializedCustomData)
    {
        return serializedCustomData.FromJson<AccountData>();
    }
}
