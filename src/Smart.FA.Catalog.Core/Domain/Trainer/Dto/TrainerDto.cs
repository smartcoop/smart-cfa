namespace Core.Domain.Dto;

public record TrainerDto(
    int Id,
    Name Name,
    string Description,
    Language DefaultLanguage);
