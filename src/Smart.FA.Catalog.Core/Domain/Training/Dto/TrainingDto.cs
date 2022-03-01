namespace Core.Domain.Dto;

public record TrainingDto
    (int TrainingId,
    short StatusId,
    string Title,
    string? Goal,
    string Language,
    int TopicId);
