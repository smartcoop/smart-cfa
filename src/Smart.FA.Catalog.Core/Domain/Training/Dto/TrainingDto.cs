namespace Core.Domain.Dto;

public record TrainingDto
    (int TrainingId,
    int StatusId,
    string Title,
    string? Goal,
    string Language);
