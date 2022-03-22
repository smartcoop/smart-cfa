using Smart.FA.Catalog.Core.Domain.User.Dto;

namespace Smart.FA.Catalog.Core.Services;

public interface IUserStrategy
{
    Task<UserDto> GetAsync(string userId);
}
