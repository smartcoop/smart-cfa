using Smart.FA.Catalog.Showcase.Domain;

namespace Smart.FA.Catalog.Showcase.Infrastructure.Data.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<int> RandomizeOrder(this IQueryable<int> query)
    {
        return query.OrderBy(id => Guid.NewGuid());
    }

    public static IQueryable<T> RandomizeOrder<T>(this IQueryable<T> query) where T : IHasId
    {
        return query.OrderBy(id => Guid.NewGuid());
    }
}
