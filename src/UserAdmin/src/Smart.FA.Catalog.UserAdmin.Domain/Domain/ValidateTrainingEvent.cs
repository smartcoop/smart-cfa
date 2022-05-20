using Smart.FA.Catalog.UserAdmin.Domain.SeedWork;

namespace Smart.FA.Catalog.UserAdmin.Domain.Domain;

public class ValidateTrainingEvent: DomainEvent
{
    public string TrainingName { get; }
    public int TrainingId { get; }
    public IEnumerable<int> TrainersId { get; }

    public ValidateTrainingEvent(string trainingName, int trainingId, IEnumerable<int> trainersId)
    {
        TrainingName = trainingName;
        TrainingId = trainingId;
        TrainersId = trainersId;
    }
}
