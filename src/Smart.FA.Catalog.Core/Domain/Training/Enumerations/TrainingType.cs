using Core.SeedWork;

namespace Core.Domain;

public class TrainingType : Enumeration
{
    public static TrainingType LanguageCourse = new(1, nameof(LanguageCourse));
    public static TrainingType Professional = new(2, nameof(Professional));
    public static TrainingType SchoolCourse = new(3, nameof(SchoolCourse));
    public static TrainingType PermanentSchoolCourse = new(4, nameof(PermanentSchoolCourse));
    public static TrainingType Other = new(5, nameof(Other));

    public TrainingType(int id, string name) : base(id, name)
    {
    }

    protected TrainingType()
    {

    }
}

public static class TrainingTypeExtensions
{
    public static bool IsTrainingAutoValidated(this IEnumerable<TrainingType> trainingTypes)
        => trainingTypes.Contains(TrainingType.LanguageCourse);
}
