using MediatR;
using Microsoft.Extensions.Logging;
using Smart.FA.Catalog.Core.Extensions;
using Smart.FA.Catalog.Core.LogEvents;
using Smart.FA.Catalog.Core.SeedWork;

namespace Smart.FA.Catalog.Infrastructure.Services;

public class DomainEventPublisher : IDomainEventPublisher
{
    private readonly ILogger<DomainEventPublisher> _logger;
    private readonly IPublisher _publisher;

    public DomainEventPublisher(ILogger<DomainEventPublisher> logger, IPublisher publisher)
    {
        _logger    = logger;
        _publisher = publisher;
    }

    /// <inheritdoc />
    public async Task PublishEntitiesEventsAsync(IEnumerable<Entity> entities)
    {
        foreach (var entity in entities)
        {
            foreach (var domainEvent in entity.DomainEvents)
            {
                _logger.LogInformation(LogEventIds.DomainEventDispatch, $"Publishing Domain Event {domainEvent.GetType().Name} {domainEvent.ToJson()} of entity {entity.GetType().Name} `{entity.Id}`.");
                await _publisher.Publish(domainEvent);
            }

            entity.ClearDomainEvents();
            _logger.LogInformation(LogEventIds.DomainEventDispatch, $"Domain events publishing of entity {entity.GetType().Name} `{entity.Id}` completed with success.");
        }
    }
}
