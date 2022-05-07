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
    /// <param name="randomIds">a boolea,=n that indicated is the result queried from the data source should be randomly picked.</param>
    /// <returns>The paginated items and the total count.</returns>
    public static async Task<(List<T> PaginatedItems, int TotalCount)> PaginateAsync<T>(this IQueryable<T> query,
        int pageNumber,
        int pageSize,
        bool randomIds = false)
        where T : IHasId
    {
        var totalCount = await query.Select(item => item.Id).Distinct().CountAsync();

        var skip = Math.Max((pageNumber - 1) * pageSize, 0);
        var take = Math.Max(pageSize, 0);

        var filteredIdsQuery = query.Select(id => id.Id);

        if (randomIds)
        {
            filteredIdsQuery = filteredIdsQuery.OrderBy(id => Guid.NewGuid()).Take(take);
        }
        else
        {
            filteredIdsQuery = filteredIdsQuery.OrderBy(id => id).Skip(skip).Take(take);
        }

        var paginatedItems = await query.Where(item => filteredIdsQuery.Contains(item.Id)).ToListAsync();

        return (paginatedItems, totalCount);
    }
}
