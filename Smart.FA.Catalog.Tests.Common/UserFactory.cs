using Core.Domain.Dto;
using Core.Domain.Enumerations;

namespace Smart.FA.Catalog.Tests.Common;

public class UserFactory
{
    public UserDto CreateClean()
    {
        return new UserDto("1", "John", "Doe", ApplicationType.Account.Name);
    }
}
