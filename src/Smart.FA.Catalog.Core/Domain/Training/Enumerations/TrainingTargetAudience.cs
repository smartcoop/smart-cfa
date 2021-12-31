using Core.SeedWork;

namespace Core.Domain;

public class TrainingTargetAudience: Enumeration
{
    public static TrainingTargetAudience Employee = new(1, nameof(Employee));
    public static TrainingTargetAudience Student = new(2, nameof(Student));
    public static TrainingTargetAudience Unemployed = new(3, nameof(Unemployed));

    private TrainingTargetAudience(int id, string name) : base(id, name)
    {
    }
}
