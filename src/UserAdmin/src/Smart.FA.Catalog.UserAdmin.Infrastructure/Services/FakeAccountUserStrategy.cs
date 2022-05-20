using Smart.FA.Catalog.UserAdmin.Domain.Domain.User.Dto;
using Smart.FA.Catalog.UserAdmin.Domain.Domain.User.Enumerations;
using Smart.FA.Catalog.UserAdmin.Domain.Services;

namespace Smart.FA.Catalog.UserAdmin.Infrastructure.Services;

public class FakeAccountUserStrategy : IUserStrategy
{
    public Task<UserDto> GetAsync(string userId)
        => Task.FromResult(new UserDto(userId, "Victor", "van Duynen", ApplicationType.Account.Name, "victor@victor.com"));
}
