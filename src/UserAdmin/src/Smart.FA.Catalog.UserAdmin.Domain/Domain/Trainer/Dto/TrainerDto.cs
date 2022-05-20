namespace Smart.FA.Catalog.UserAdmin.Domain.Domain.Dto;

public record TrainerDto(
    int Id,
    string FirstName,
    string LastName,
    string Biography,
    string Title,
    string DefaultLanguage);
