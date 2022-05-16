using Microsoft.EntityFrameworkCore;
using Smart.FA.Catalog.Showcase.Domain;

namespace Smart.FA.Catalog.Showcase.Infrastructure.Data.Extensions;

public static class PaginationExtensions
{
    /// <summary>
    /// Applies pagination on data source where the data type is <see cref="IHasId" />.
    /// </summary>
    /// <typeparam name="T">The type of the data in the data source.</typeparam>
    /// <param name="query"><see cref="IQueryable{T}" /> on which the operation will be performed.</param>
    /// <param name="pageNumber">Current's page number.</param>
    /// <param name="pageSize">Number of item per page.</param>
    /// <param name="randomIds">a boolean that indicates if the result queried from the data source should be randomly picked.</param>
    /// <returns>The paginated items and the total count.</returns>
    public static async Task<(List<T> PaginatedItems, int TotalCount)> PaginateAsync<T>(this IQueryable<T> query,
        int pageNumber,
        int pageSize,
        bool randomIds = false)
        where T : IHasId
    {
        // Build a query that returns every entity ids.
        var idsQuery = query.Select(item => item.Id).Distinct();

        // Proceed to compute the total.
        var totalCount = await idsQuery.CountAsync();

        // Make sure to have the skip and take greater or equal to zero.
        // Negative values produce an exception
        var skip = Math.Max((pageNumber - 1) * pageSize, 0);
        var take = Math.Max(pageSize, 0);

        if (randomIds)
        {
            idsQuery = idsQuery.RandomizeOrder();
        }

        var filteredIdsQuery = idsQuery.Skip(skip).Take(take);

        query = query.Where(item => filteredIdsQuery.Contains(item.Id));

        // Make sure to keep consistent ordering.
        var orderedList = await query.OrderBy(item => item.Id).ToListAsync();
        
        return (orderedList, totalCount);
    }
}
