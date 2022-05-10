using Microsoft.EntityFrameworkCore;
using Smart.FA.Catalog.Shared.Collections;

namespace Smart.FA.Catalog.Infrastructure.Extensions;

public static class PagedListExtension
{
    public static async Task<PagedList<T>> PaginateAsync<T>(this IQueryable<T> source, PageItem pageItem, CancellationToken cancellationToken)
    {
        var count = await source.CountAsync();
        var items = await source.Skip(pageItem.Offset).Take(pageItem.PageSize).ToListAsync();
        return new PagedList<T>(items, pageItem, count);
    }
}
