namespace Smart.FA.Catalog.Shared.Domain.Enumerations.Training;

public class TrainingSlotNumberType: Enumeration
{
    public static readonly TrainingSlotNumberType Group = new(1, nameof(Group));
    public static readonly TrainingSlotNumberType Single = new(2, nameof(Single));

    public TrainingSlotNumberType(int id, string name): base(id, name)
    {

    }
}
