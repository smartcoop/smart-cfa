namespace Core.Domain.Dto;

public record TrainerDto(
    int Id,
    string FirstName,
    string LastName,
    string? Description,
    string DefaultLanguage);
