namespace Smart.FA.Catalog.Core.Domain.Authorization;

public class SuperUser
{
    public string UserId { get; init; } = null!;

    private SuperUser()
    {
    }

    public static SuperUser Create(string userId)
    {
        return new SuperUser()
        {
            UserId = userId
        };
    }
}
