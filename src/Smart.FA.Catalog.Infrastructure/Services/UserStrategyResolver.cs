using Core.Domain.Enumerations;
using Core.Services;

namespace Infrastructure.Services;

public class UserStrategyResolver
{
    private readonly string _accountConnectionString;

    public UserStrategyResolver(string accountConnectionString)
    {
        _accountConnectionString = accountConnectionString;
    }
    public IUserStrategy Resolve(ApplicationType applicationType)
    {
        //Right now there is only the account strategy so next line is useless
        // if (applicationType == ApplicationType.Account) return new AccountUserStrategy(_accountConnectionString);

        return new AccountUserStrategy(_accountConnectionString);
    }
}
