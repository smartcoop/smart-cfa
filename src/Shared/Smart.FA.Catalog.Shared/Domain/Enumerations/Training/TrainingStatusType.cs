using Smart.FA.Catalog.Shared.Domain.Enumerations.Common;

namespace Smart.FA.Catalog.Shared.Domain.Enumerations.Training;

public class TrainingStatusType : Enumeration<TrainingStatusType>
{
    public static readonly TrainingStatusType Draft = new(1, "Draft");
    public static readonly TrainingStatusType WaitingForValidation = new(2, "PendingValidation");
    public static readonly TrainingStatusType Published = new(3, "Published");
    public static readonly TrainingStatusType Cancelled = new(4, "Cancelled");

    private TrainingStatusType(int id, string name) : base(id, name)
    {
    }

    protected TrainingStatusType()
    {

    }
}
