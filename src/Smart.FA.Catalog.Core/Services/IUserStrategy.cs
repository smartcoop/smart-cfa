using Core.Domain.Dto;

namespace Core.Services;

public interface IUserStrategy
{
    Task<UserDto> GetAsync(string userId);
}
