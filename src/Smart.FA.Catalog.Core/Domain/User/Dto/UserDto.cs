using Core.Domain.Enumerations;

namespace Core.Domain.Dto;

public record UserDto(string UserId, string FirstName, string LastName, string ApplicationType);
