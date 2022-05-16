namespace Smart.FA.Catalog.AccountSimulator;

public interface IAccountDataHeaderSerializer
{
    string Serialize(AccountData accountData);
    AccountData GetByUserId(string userId);
}
