using Core.SeedWork;

namespace Core.Domain;

public class Trainer : Entity, IAggregateRoot
{
    private readonly List<TrainerEnrollment> _enrollments = new();
    #region Properties

    public string FirstName { get; }
    public string LastName { get; }
    public string Description { get; }
    public IReadOnlyCollection<TrainerEnrollment> Enrollments => _enrollments;

    #endregion

    public Trainer(string firstName, string lastName, string description )
    {
        FirstName = firstName;
        LastName = lastName;
        Description = description;
    }
}
