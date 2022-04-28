namespace Smart.FA.Catalog.Core.Domain.Authorization;

public class SuperAdmin
{
    public string UserId { get; init; } = null!;

    private SuperAdmin()
    {
    }

    public static SuperAdmin Create(string userId)
    {
        return new SuperAdmin()
        {
            UserId = userId
        };
    }
}
