using Core.SeedWork;

namespace Core.Domain;

public class Trainer : Entity, IAggregateRoot
{
    #region Private fields

    private readonly List<TrainerEnrollment> _enrollments = new();

    #endregion

    #region Properties

    public virtual Name Name { get; private set; }

    public string Description { get; private set; }

    public Language DefaultLanguage { get; private set; }

    public virtual IReadOnlyCollection<TrainerEnrollment> Enrollments => _enrollments;

    #endregion

    #region Constructors

    public Trainer(Name name, string description, Language defaultLanguage)
    {
        ChangeDefaultLanguage(defaultLanguage);
        Rename(name);
        UpdateDescription(description);
    }

    protected Trainer()
    {
    }

    #endregion

    #region Methods

    public void UpdateDescription(string description)
    {
        Guard.AgainstNull(description, nameof(description));
        Guard.Requires(() => description.Length <= 2000
            , "Description is too long");
        Description = description;
    }

    public void EnrollIn(Training training)
    {
        Guard.Requires(() =>! _enrollments.Select(enrollment => enrollment.Training).Contains(training),
            "The trainer is already enrolled in that training");
        _enrollments.Add(new TrainerEnrollment(training, this));
    }

    public void DisenrollFrom(Training training)
    {
        var trainingNumberRemoved = _enrollments.RemoveAll(enrollment => enrollment.Training == training);
        Guard.Ensures(() => trainingNumberRemoved != 0, "The trainer has never enrolled in that training");
    }

    public List<Training> GetTrainings()
        => Enrollments.Any() ? Enrollments.Select(enrollment => enrollment.Training).ToList() : new List<Training>();

    public void Rename(Name name) => Name = name;
    public void ChangeDefaultLanguage(Language language) => DefaultLanguage = language;

    #endregion
}
