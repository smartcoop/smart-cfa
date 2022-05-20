using Microsoft.EntityFrameworkCore;
using Smart.FA.Catalog.UserAdmin.Domain.SeedWork;

namespace Smart.FA.Catalog.UserAdmin.Infrastructure.Extensions;

public static class DbSetExtensions
{
    /// <summary>
    /// Retrieves the last created  <see cref="Entity" /> or if the list is empty, its default value.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity to be retrieved.</typeparam>
    /// <param name="entityDbSet"> <see cref="DbSet{TEntity}" /> The set of entities on which will be performed the operation.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns> A task representing the asynchronous operations. The task result is a <see cref="Nullable{T}" /> with T being an <see cref="Entity" />.</returns>
    public static async Task<TEntity?> GetLatestCreatedOrDefaultAsync<TEntity>(this DbSet<TEntity> entityDbSet, CancellationToken cancellationToken = default) where TEntity : Entity
        => await entityDbSet.OrderByDescending(entity => entity.CreatedAt).FirstOrDefaultAsync(cancellationToken);
}
