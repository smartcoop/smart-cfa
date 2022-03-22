using Smart.FA.Catalog.Core.SeedWork;

namespace Smart.FA.Catalog.Core.Domain.Enumerations;

public class TrainingType : Enumeration
{
    public static readonly TrainingType LanguageCourse = new(1, nameof(LanguageCourse));
    public static readonly TrainingType Professional = new(2, nameof(Professional));
    public static readonly TrainingType SchoolCourse = new(3, nameof(SchoolCourse));
    public static readonly TrainingType PermanentSchoolCourse = new(4, nameof(PermanentSchoolCourse));

    private TrainingType(int id, string name) : base(id, name)
    {
    }

    protected TrainingType()
    {

    }
}

public static class TrainingTypeExtensions
{
    public static bool IsTrainingAutoValidated(this IEnumerable<TrainingIdentity> trainingTypes)
        => trainingTypes.Select(identity => identity.TrainingType).Contains(TrainingType.LanguageCourse);
}
