namespace Smart.FA.Catalog.Shared.Extensions;

public static class CollectionExtensions
{
    public static void AddIf<T>(this ICollection<T> collection, Func<bool> predicate, T item)
    {
        if (predicate.Invoke())
            collection.Add(item);
    }

    public static void AddIf<T>(this ICollection<T> collection, Func<T, bool> predicate, T item)
    {
        if (predicate.Invoke(item))
            collection.Add(item);
    }
}
