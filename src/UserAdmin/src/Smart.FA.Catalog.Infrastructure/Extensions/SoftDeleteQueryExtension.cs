using Microsoft.EntityFrameworkCore;
using Smart.FA.Catalog.Core.Domain;

namespace Smart.FA.Catalog.Infrastructure.Extensions;

/// <summary>
/// Add global query filter for soft deletable class implementing the <see cref="ISoftDeletable"/> interface
/// </summary>
public static class SoftDeleteQueryExtension
{
    public static void AddSoftDeleteQueryFilter(this ModelBuilder modelBuilder)
    {
        modelBuilder.SetFilterQueryOnEntityDerivedFrom<ISoftDeletable>(entity => entity.SoftDeletedAt == null);
    }
}
