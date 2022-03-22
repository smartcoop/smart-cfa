using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Smart.FA.Catalog.Core.Domain;
using Smart.FA.Catalog.Core.SeedWork;
using Smart.FA.Catalog.Infrastructure.Extensions;

namespace Smart.FA.Catalog.Infrastructure.Persistence;

public class CatalogContext : DbContext
{
    private readonly EventDispatcher? _eventDispatcher;

    public CatalogContext
    (
        DbContextOptions<CatalogContext> contextOptions
        , EventDispatcher?               eventDispatcher
    ) : base(contextOptions)
    {
        _eventDispatcher = eventDispatcher;
    }

    public DbSet<Trainer> Trainers { get; set; } = null!;
    public DbSet<Training> Trainings { get; set; } = null!;
    public DbSet<TrainerAssignment> TrainerAssignments { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        modelBuilder.ApplyDateTimeConverters();
    }

    public override int SaveChanges()
    {
        AddDateTimeToEntities();
        RemoveEnumerationsFromTracker();
        DispatchEventFromEntities();
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = new CancellationToken())
    {
        AddDateTimeToEntities();
        RemoveEnumerationsFromTracker();
        DispatchEventFromEntities();
        return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
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

    private void DispatchEventFromEntities()
    {
        var entries = ChangeTracker
            .Entries<Entity>()
            .Select(entry => entry.Entity)
            .Where(entity => entity.DomainEvents.Any());
        foreach (var entity in entries)
        {
            _eventDispatcher!.Dispatch(entity.DomainEvents);
            entity.ClearDomainEvents();
        }
    }
}
