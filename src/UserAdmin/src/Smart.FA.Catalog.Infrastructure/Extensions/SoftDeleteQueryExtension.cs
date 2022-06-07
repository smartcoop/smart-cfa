using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Smart.FA.Catalog.Core.Domain;

namespace Smart.FA.Catalog.Infrastructure.Extensions;

/// <summary>
/// Add global query filter for soft deletable class implementing the <see cref="ISoftDeletable"/> interface
/// </summary>
public static class SoftDeleteQueryExtension
{
    public static void AddSoftDeleteQueryFilter(this ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            // Does the entity implements IEFSoftDelete interface? (Note the Entity base class of all entities implement the interface and so all entities do, too)
            if (typeof(ISoftDeletable).IsAssignableFrom(entityType.ClrType))
            {
                ApplySoftDeleteQueryFilter(entityType);
            }
        }
    }

    private static void ApplySoftDeleteQueryFilter(IMutableEntityType entityType)
    {
        // Get and call the static Lambda expression which filters soft deletable entities
        var methodToCall = typeof(SoftDeleteQueryExtension).GetMethod(nameof(GetSoftDeleteFilter), BindingFlags.NonPublic | BindingFlags.Static)!.MakeGenericMethod(entityType.ClrType);
        var filter = methodToCall.Invoke(null, Array.Empty<object>());
        // Set the filter to be automatically applied on top of any queries on the entity
        entityType.SetQueryFilter((LambdaExpression)filter!);
        // Index on the soft deleted flag to increase database performance
        entityType.AddIndex(entityType.FindProperty(nameof(ISoftDeletable.IsSoftDeleted))!);
    }

    /// <summary>
    /// Filter the entity on the soft delete flag
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    private static LambdaExpression GetSoftDeleteFilter<TEntity>() where TEntity : class, ISoftDeletable
    {
        Expression<Func<TEntity, bool>> filter = x => !x.IsSoftDeleted;
        return filter;
    }
}
