namespace Smart.FA.Catalog.UserAdmin.Domain.Domain.Dto;

// Because of some restrictions with Dapper*, we cannot use a record here
// *Dapper split-on feature seems to only support a mutable list of topic ids in its constructor
public class TrainingDto
{
    public int TrainingId { get; }
    public int TrainingStatusTypeId { get; }
    public string Title { get; }
    public string? Goal { get; }
    public string Language { get; }
    public List<int>? TopicIds { get; }

    public TrainingDto(int trainingId,
        int trainingStatusTypeId,
        string title,
        string? goal,
        string language,
        List<int> topicIds)
    {
        TrainingId = trainingId;
        TrainingStatusTypeId = trainingStatusTypeId;
        Title = title;
        Goal = goal;
        Language = language;
        TopicIds = topicIds;
    }

    public TrainingDto(int trainingId,
        int trainingStatusTypeId,
        string title,
        string? goal,
        string language)
    {
        TrainingId = trainingId;
        TrainingStatusTypeId = trainingStatusTypeId;
        Title = title;
        Goal = goal;
        Language = language;
        TopicIds = new List<int>();
    }
}
