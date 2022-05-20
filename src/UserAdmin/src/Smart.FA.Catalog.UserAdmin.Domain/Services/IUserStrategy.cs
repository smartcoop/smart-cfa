using Smart.FA.Catalog.UserAdmin.Domain.Domain.User.Dto;

namespace Smart.FA.Catalog.UserAdmin.Domain.Services;

public interface IUserStrategy
{
    Task<UserDto> GetAsync(string userId);
}
