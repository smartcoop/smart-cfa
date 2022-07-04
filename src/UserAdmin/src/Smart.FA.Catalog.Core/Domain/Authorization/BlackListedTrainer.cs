using Smart.FA.Catalog.Core.SeedWork;

namespace Smart.FA.Catalog.Core.Domain.Authorization;

public class BlackListedTrainer : IAggregateRoot
{
    public int TrainerId { get; }

    protected BlackListedTrainer()
    {
    }

    public BlackListedTrainer(int trainerId)
    {
        TrainerId = trainerId;
    }
}
