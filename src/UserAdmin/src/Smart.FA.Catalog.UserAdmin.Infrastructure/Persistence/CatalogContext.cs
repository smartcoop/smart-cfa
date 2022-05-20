using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Smart.FA.Catalog.Shared.Domain.Enumerations.Common;
using Smart.FA.Catalog.UserAdmin.Domain.Domain;
using Smart.FA.Catalog.UserAdmin.Domain.Domain.Authorization;
using Smart.FA.Catalog.UserAdmin.Domain.SeedWork;
using Smart.FA.Catalog.UserAdmin.Domain.Services;
using Smart.FA.Catalog.UserAdmin.Infrastructure.Extensions;
using Smart.FA.Catalog.UserAdmin.Infrastructure.Persistence.Extensions;

namespace Smart.FA.Catalog.UserAdmin.Infrastructure.Persistence;

public class CatalogContext : DbContext
{
    private readonly IDomainEventPublisher _eventPublisher;
    private readonly IUserIdentity _userIdentity;

    public CatalogContext(DbContextOptions<CatalogContext> contextOptions, IDomainEventPublisher eventPublisher, IUserIdentity userIdentity) : base(contextOptions)
    {
        _eventPublisher = eventPublisher;
        _userIdentity = userIdentity;
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
        UpdateAuditableEntitiesData();
        DetachEnumerations();
        var numberOfEntitiesWritten = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        await PublishEntityDomainEventsAsync();
        return numberOfEntitiesWritten;
    }

    private void DetachEnumerations()
    {
        ChangeTracker
            .Entries<IEnumeration>()
            .ToList()
            .ForEach(enumEntry => enumEntry.State = EntityState.Detached);
    }

    private void UpdateAuditableEntitiesData()
    {
        var entries = ChangeTracker
            .Entries<Entity>()
            .Where(entry => entry.State is EntityState.Added or EntityState.Modified);

        entries.UpdateAuditableEntitiesData(_userIdentity.Id);
    }

    private Task PublishEntityDomainEventsAsync()
    {
        var entitiesWithEvents = ChangeTracker
            .Entries<Entity>()
            .Select(entry => entry.Entity)
            .Where(entity => entity.DomainEvents.Any());

        return _eventPublisher.PublishEntitiesEventsAsync(entitiesWithEvents);
    }
}
