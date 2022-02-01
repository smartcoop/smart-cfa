using Core.Domain.Dto;
using Core.Domain.Enumerations;
using Core.Services;

namespace Infrastructure.Services;

public class FakeAccountUserStrategy : IUserStrategy
{
    public Task<UserDto> GetAsync(string userId)
        => Task.FromResult(new UserDto(userId, "Victor", "van Duynen", ApplicationType.Account.Name));
}
