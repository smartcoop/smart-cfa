using Smart.FA.Catalog.Core.Domain.User.Dto;
using Smart.FA.Catalog.Core.Domain.User.Enumerations;
using Smart.FA.Catalog.Core.Services;

namespace Smart.FA.Catalog.Infrastructure.Services;

public class FakeAccountUserStrategy : IUserStrategy
{
    public Task<UserDto> GetAsync(string userId)
        => Task.FromResult(new UserDto(userId, "Victor", "van Duynen", ApplicationType.Account.Name));
}
