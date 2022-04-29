using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Smart.FA.Catalog.Core.Domain;
using Smart.FA.Catalog.Core.Domain.Authorization;
using Smart.FA.Catalog.Core.SeedWork;
using Smart.FA.Catalog.Infrastructure.Extensions;
using Smart.FA.Catalog.Shared.Domain.Enumerations;

namespace Smart.FA.Catalog.Infrastructure.Persistence;

public class CatalogContext : DbContext
{
    private readonly IDomainEventPublisher _eventPublisher;

    public CatalogContext(DbContextOptions<CatalogContext> contextOptions, IDomainEventPublisher eventPublisher) : base(contextOptions)
    {
        _eventPublisher = eventPublisher;
    }

    public DbSet<TrainerAssignment> TrainerAssignments { get; set; } = null!;

    public DbSet<Trainer> Trainers { get; set; } = null!;

    public DbSet<Training> Trainings { get; set; } = null!;

    public DbSet<SuperUser> SuperUsers { get; set; } = null!;

    public DbSet<UserChartRevision> UserChartRevisions { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Cfa");
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        modelBuilder.ApplyDateTimeConverters();
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        return SaveChangesAsync(acceptAllChangesOnSuccess).GetAwaiter().GetResult();
    }

    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        AddDateTimeToEntities();
        RemoveEnumerationsFromTracker();
        var numberOfEntitiesWritten = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        await PublishEntityDomainEventsAsync();
        return numberOfEntitiesWritten;
    }

    private void RemoveEnumerationsFromTracker()
    {
        ChangeTracker
            .Entries<Enumeration>()
            .ToList()
            .ForEach(ee => ee.State = EntityState.Detached);
    }

    private void AddDateTimeToEntities()
    {
        var entries = ChangeTracker
            .Entries<Entity>()
            .Where(entry => entry.State is EntityState.Added or EntityState.Modified);

        foreach (var entityEntry in entries)
        {
            entityEntry.Entity.LastModifiedAt = DateTime.UtcNow;

            if (entityEntry.State == EntityState.Added)
            {
                entityEntry.Entity.CreatedAt = DateTime.UtcNow;
            }
        }
    }

    private Task PublishEntityDomainEventsAsync()
    {
        var entitiesWithEvents = ChangeTracker
            .Entries<Entity>()
            .Select(entry => entry.Entity)
            .Where(entity => entity.DomainEvents.Count == 0);

        return _eventPublisher.PublishEntitiesEventsAsync(entitiesWithEvents);
    }
}
