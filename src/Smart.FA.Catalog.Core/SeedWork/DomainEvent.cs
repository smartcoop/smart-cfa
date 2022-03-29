using MediatR;

namespace Smart.FA.Catalog.Core.SeedWork;

public abstract class DomainEvent : IDomainEvent
{
    public DateTime OccurredAt { get; } = DateTime.UtcNow;
}

public interface IDomainEvent : INotification
{
    public DateTime OccurredAt { get; }
}
