namespace Smart.FA.Catalog.UserAdmin.Domain.SeedWork;

public interface IDomainEventPublisher
{
    /// <summary>
    /// Publishes every <paramref name="entities" />'s <see cref="IDomainEvent" />.
    /// Each <see cref="IDomainEvent" /> published is removed from the entity to whom it belongs to.
    /// This operation is not safe, this means that if any exception is raised during the process,
    /// the execution will stop.
    /// </summary>
    /// <param name="entities">The entities whose domain events needs to be published.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task PublishEntitiesEventsAsync(IEnumerable<Entity> entities);
}
