using Core.Domain.Enumerations;
using Core.SeedWork;
using CSharpFunctionalExtensions;
using ValueObject = CSharpFunctionalExtensions.ValueObject;

namespace Core.Domain;

public class TrainerIdentity : ValueObject
{
    public string UserId { get; } = null!;
    public int ApplicationTypeId { get; }

    protected TrainerIdentity()
    {

    }
    private TrainerIdentity(string userId, ApplicationType applicationType):base()
    {
        UserId = userId;
        ApplicationTypeId = applicationType.Id;
    }

    public static Result<TrainerIdentity> Create(string userId, ApplicationType applicationType)
    {
        if (string.IsNullOrWhiteSpace(userId))
            return Result.Failure<TrainerIdentity>("User id should not be empty");

        return Result.Success(new TrainerIdentity(userId, applicationType));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return UserId;
        yield return ApplicationTypeId;
    }
}
