namespace Smart.FA.Catalog.Core.Domain.Authorization;

public class SuperUser
{
    public int TrainerId { get; init; }

    private SuperUser()
    {
    }

    public static SuperUser Create(int trainerId)
    {
        return new SuperUser()
        {
            TrainerId = trainerId
        };
    }
}
