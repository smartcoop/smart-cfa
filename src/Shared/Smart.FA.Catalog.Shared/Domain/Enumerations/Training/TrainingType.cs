namespace Smart.FA.Catalog.Shared.Domain.Enumerations.Training;

public class TrainingType : Enumeration
{
    public static readonly TrainingType LanguageCourse = new(1, nameof(LanguageCourse));
    public static readonly TrainingType Professional = new(2, nameof(Professional));
    public static readonly TrainingType ScholarTraining = new(3, nameof(ScholarTraining));
    //public static readonly TrainingType PermanentSchoolCourse = new(4, nameof(PermanentSchoolCourse));

    private TrainingType(int id, string name) : base(id, name)
    {
    }

    protected TrainingType()
    {

    }
}
