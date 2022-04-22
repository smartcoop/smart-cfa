namespace Smart.FA.Catalog.Shared.Domain.Enumerations.Training;

public class TrainingStatusType : Enumeration
{
    public static readonly TrainingStatusType Draft = new(1, "Draft");
    public static readonly TrainingStatusType WaitingForValidation = new(2, "PendingValidation");
    public static readonly TrainingStatusType Validated = new(3, "Validated");
    public static readonly TrainingStatusType Cancelled = new(4, "Cancelled");

    private TrainingStatusType(int id, string name) : base(id, name)
    {
    }

    protected TrainingStatusType()
    {

    }
}
