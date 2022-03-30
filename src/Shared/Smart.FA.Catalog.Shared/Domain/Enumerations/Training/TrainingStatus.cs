namespace Smart.FA.Catalog.Shared.Domain.Enumerations.Training;

public class TrainingStatus : Enumeration
{
    public static readonly TrainingStatus Draft = new(1, "Draft");
    public static readonly TrainingStatus WaitingForValidation = new(2, "PendingValidation");
    public static readonly TrainingStatus Validated = new(3, "Validated");
    public static readonly TrainingStatus Cancelled = new(4, "Cancelled");

    private TrainingStatus(int id, string name) : base(id, name)
    {
    }

    protected TrainingStatus()
    {

    }
}
