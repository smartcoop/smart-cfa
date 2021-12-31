using Core.SeedWork;

namespace Core.Domain;

public class TrainingStatus : Enumeration
{
    public static readonly TrainingStatus Draft = new(1, "Draft");
    public static readonly TrainingStatus WaitingForValidation = new(2, "WaitingForValidation");
    public static readonly TrainingStatus Validated = new(3, "Validated");

    private TrainingStatus(int id, string name) : base(id, name)
    {
    }

    protected TrainingStatus()
    {

    }
}

public static class TrainingStatusExtensions
{
    public static TrainingStatus Validate(this TrainingStatus status, IEnumerable<TrainingType> trainingTypes)
     => trainingTypes.IsTrainingAutoValidated() ? TrainingStatus.Validated : TrainingStatus.WaitingForValidation;
}
