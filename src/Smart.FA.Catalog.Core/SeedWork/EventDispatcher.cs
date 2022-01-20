using Core.Domain;

namespace Core.SeedWork;

public class EventDispatcher
{
    private readonly MessageBus _messageBus;

    public EventDispatcher(MessageBus messageBus)
    {
        _messageBus = messageBus;
    }

    public void Dispatch(IEnumerable<DomainEvent> domainEvents)
    {
        foreach (var domainEvent in domainEvents)
        {
            Dispatch(domainEvent);
        }
    }

    public void Dispatch(DomainEvent domainEvent)
    {
        switch (domainEvent)
        {
            case ValidateTrainingEvent validation:
                _messageBus.ValidateTrainingMessage(validation.TrainingId,
                    validation.TrainingName, validation.TrainersId);
                break;
            default:
                throw new Exception($"Unknown event type: '{domainEvent.GetType()}'");
        }
    }
}
