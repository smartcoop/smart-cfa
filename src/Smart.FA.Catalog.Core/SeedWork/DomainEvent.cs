using MediatR;

namespace Core.SeedWork;

public abstract class DomainEvent : INotification
{
    public DateTime OccurredAt { get; }

    protected DomainEvent()
    {
        OccurredAt = DateTime.Now;
    }
}
