using Core.SeedWork;

namespace Core.Domain.Enumerations;

public class TrainingTargetAudience: Enumeration
{
    public static readonly TrainingTargetAudience Employee = new(1, nameof(Employee));
    public static readonly TrainingTargetAudience Student = new(2, nameof(Student));
    public static readonly TrainingTargetAudience Unemployed = new(3, nameof(Unemployed));

    protected TrainingTargetAudience(int id, string name) : base(id, name)
    {
    }
}
