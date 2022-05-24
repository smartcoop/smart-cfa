using CSharpFunctionalExtensions;
using Smart.FA.Catalog.Core.Domain.User.Enumerations;
using Smart.FA.Catalog.Core.Exceptions;
using ValueObject = CSharpFunctionalExtensions.ValueObject;

namespace Smart.FA.Catalog.Core.Domain.ValueObjects;

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
