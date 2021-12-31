using Core.SeedWork;

namespace Core.Domain;

public class TrainingSlotNumberType: Enumeration
{
    public static TrainingSlotNumberType Group = new TrainingSlotNumberType(1, nameof(Group));
    public static TrainingSlotNumberType Single = new TrainingSlotNumberType(2, nameof(Single));

    public TrainingSlotNumberType(int id, string name): base(id, name)
    {

    }
}
