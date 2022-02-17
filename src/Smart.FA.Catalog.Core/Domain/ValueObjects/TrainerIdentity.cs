using Core.Domain.Enumerations;
using Core.Exceptions;
using Core.SeedWork;
using CSharpFunctionalExtensions;
using ValueObject = CSharpFunctionalExtensions.ValueObject;

namespace Core.Domain;

public class TrainerIdentity : ValueObject
{
    public string UserId { get; } = null!;
    public int ApplicationTypeId { get; }

    private TrainerIdentity(string userId, ApplicationType applicationType):base()
    {
        UserId = userId;
        ApplicationTypeId = applicationType.Id;
    }

    protected TrainerIdentity() { }

    public static Result<TrainerIdentity, Error> Create(string userId, ApplicationType applicationType)
    {
        if (string.IsNullOrWhiteSpace(userId))
            return Errors.General.ValueIsRequired();

        return new TrainerIdentity(userId, applicationType);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return UserId;
        yield return ApplicationTypeId;
    }
}
