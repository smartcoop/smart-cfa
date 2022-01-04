using Core.SeedWork;

namespace Core.Domain;

public class Trainer : Entity, IAggregateRoot
{
    #region Private fields

    private readonly List<TrainerEnrollment> _enrollments = new();
    private string _description;
    private string _firstName;
    private string _lastName;
    private string _defaultLanguage;

    #endregion

    #region Properties

    public string FirstName
    {
        get => _firstName;
        set
        {
            if (value.Length > 150) throw new Exception($"{nameof(FirstName)} has a maximum value of 150 characters");
            _firstName = value;
        }
    }

    public string LastName
    {
        get => _lastName;
        set
        {
            if (value.Length > 150) throw new Exception($"{nameof(LastName)} has a maximum value of 150 characters");
            _lastName = value;
        }
    }

    public string Description
    {
        get => _description;
        set
        {
            if (value.Length > 150) throw new Exception($"{nameof(Description)} has a maximum value of 150 characters");
            _description = value;
        }
    }

    public string DefaultLanguage
    {
        get => _defaultLanguage;
        set
        {
            if (value.Length > 2) throw new Exception($"{nameof(DefaultLanguage)} has a maximum value of 2 characters");
            _description = value;
        }
    }

    public virtual IReadOnlyCollection<TrainerEnrollment> Enrollments => _enrollments;

    #endregion

    #region Constructors

    public Trainer(string firstName, string lastName, string description, string defaultLanguage)
    {
        _defaultLanguage = defaultLanguage;
        FirstName = firstName;
        LastName = lastName;
        Description = description;
    }

    protected Trainer()
    {
    }

    #endregion

    #region Methods

    public void UpdateDescription(string description)
    {
        Description = description;
    }

    public void EnrollIn(Training training)
    {
        if (_enrollments.Select(enrollment => enrollment.Training).Contains(training)) throw new Exception();
        _enrollments.Add(new TrainerEnrollment(training, this));
    }

    public void DisenrollFrom(Training training)
    {
        var trainingNumberRemoved =_enrollments.RemoveAll(enrollment => enrollment.Training == training);
        if (trainingNumberRemoved == 0) throw new Exception("The trainer has never enrolled in that training");
    }

    #endregion
}
