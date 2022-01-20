namespace Core.SeedWork;

public class MessageBus
{
    private readonly IBus _bus;

    public MessageBus(IBus bus)
    {
        _bus = bus;
    }

    public void ValidateTrainingMessage(int trainingId, string trainingName, IEnumerable<int> trainerIds)
    {
        _bus.Send(
            $"training {trainingName} Id {trainingId} for trainer{(trainerIds.Any() ? "s" : "")} id {string.Join(", ", trainerIds)} needs some validation");
    }
}
