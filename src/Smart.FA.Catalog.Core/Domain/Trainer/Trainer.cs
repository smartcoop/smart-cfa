using Core.SeedWork;

namespace Core.Domain;

public class Trainer : Entity, IAggregateRoot
{
    #region Private fields

    private readonly List<TrainerEnrollment> _enrollments = new();

    #endregion

    #region Properties

    public virtual Name Name { get; private set; } = null!;

    public string Biography { get; private set; } = null!;
    public string Title { get; set; } = null!;

    public Language DefaultLanguage { get; private set; } = null!;

    public TrainerIdentity Identity { get; } = null!;
    public virtual IReadOnlyCollection<TrainerEnrollment> Enrollments => _enrollments;

    #endregion

    #region Constructors

    public Trainer(Name name, TrainerIdentity identity, string title, string description, Language defaultLanguage)
    {
        Identity = identity;
        ChangeDefaultLanguage(defaultLanguage);
        Rename(name);
        UpdateBiography(description);
        UpdateTitle(title);
    }

    protected Trainer()
    {
    }

    #endregion

    #region Methods

    public void UpdateBiography(string newBiography)
    {
        Guard.AgainstNull(newBiography, nameof(newBiography));
        Guard.Requires(() => newBiography.Length <= 1500
            , "Biography is too long");
        Biography = newBiography;
    }

    public void UpdateTitle(string newTitle)
    {
        Guard.AgainstNull(newTitle, nameof(newTitle));
        Guard.Requires(() => newTitle.Length <= 150
            , "Title is too long");
        Title = newTitle;
    }

    public void EnrollIn(Training training)
    {
        Guard.Requires(() => !_enrollments.Select(enrollment => enrollment.Training).Contains(training),
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
