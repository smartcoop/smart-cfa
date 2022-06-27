using Smart.FA.Catalog.Core.Domain.ValueObjects;

namespace Smart.FA.Catalog.Core.Domain.Authorization;

public class BlackListedUser
{
    public string UserId { get; }
    public int ApplicationTypeId { get; }

    protected BlackListedUser()
    {
    }

    public BlackListedUser(TrainerIdentity trainerIdentity)
    {
        UserId = trainerIdentity.UserId;
        ApplicationTypeId = trainerIdentity.ApplicationTypeId;
    }
}
