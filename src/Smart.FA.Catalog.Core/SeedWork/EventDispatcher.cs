using Core.Domain;
using MediatR;

namespace Core.SeedWork;

public class EventDispatcher
{
    private readonly IMediator _mediator;
    public EventDispatcher(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Dispatch(IEnumerable<DomainEvent> domainEvents)
    {
        foreach (var domainEvent in domainEvents)
        {
           await Dispatch(domainEvent);
        }
    }

    private async Task Dispatch(DomainEvent domainEvent)
    {
        await _mediator.Publish(domainEvent);
    }
}
