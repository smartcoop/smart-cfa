using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Smart.FA.Catalog.Core.SeedWork;

namespace Smart.FA.Catalog.Infrastructure.Persistence.Extensions;

public static class EntryEntityAuditableDateUpdate
{
    /// <summary>
    /// Updates a collection of <see cref="Entity" /> to keep their data auditable.
    /// </summary>
    /// <param name="entityEntries">A collection of <see cref="EntityEntry{TEntity}"/>.</param>
    /// <param name="userId">The id of the user doing the current operation.</param>
    public static void UpdateAuditableEntitiesData(this IEnumerable<EntityEntry<Entity>> entityEntries, int userId)
    {
        foreach (var entityEntry in entityEntries)
        {
            entityEntry.UpdateEntityAuditableData(userId);
        }
    }

    private static void UpdateEntityAuditableData(this EntityEntry<Entity> entityEntry, int userId)
    {
        if (entityEntry.State == EntityState.Added)
        {
            entityEntry.SetAddedEntitiesData(userId);
        }

        if (entityEntry.State == EntityState.Modified)
        {
            entityEntry.SetModifiedEntitiesData(userId);
        }
    }

    private static void SetAddedEntitiesData(this EntityEntry<Entity> entityEntry, int userId)
    {
        var now = DateTime.UtcNow;

        // Sets creation, modification dates and creator and last updater.
        entityEntry.Property<DateTime>(nameof(Entity.CreatedAt)).CurrentValue      = now;
        entityEntry.Property<DateTime>(nameof(Entity.LastModifiedAt)).CurrentValue = now;
        entityEntry.Property<int>(nameof(Entity.CreatedBy)).CurrentValue           = userId;
        entityEntry.Property<int>(nameof(Entity.LastModifiedBy)).CurrentValue      = userId;
    }

    private static void SetModifiedEntitiesData(this EntityEntry<Entity> entityEntry, int userId)
    {
        // Sets modification data (i.e. modification date and creator id).
        entityEntry.Property<DateTime>(nameof(Entity.LastModifiedAt)).CurrentValue = DateTime.UtcNow;
        entityEntry.Property<int>(nameof(Entity.LastModifiedBy)).CurrentValue      = userId;

        // This prevents that someone overrides creation date and creator id when modifying an entity.
        entityEntry.Property<DateTime>(nameof(Entity.CreatedAt)).IsModified = false;
        entityEntry.Property<int>(nameof(Entity.CreatedBy)).IsModified      = false;
    }
}
