using System.Collections;

namespace Smart.FA.Catalog.UserAdmin.Domain.Extensions;

public static class CollectionExtensions
{
    /// <summary>
    /// Determines if there is an element of type <typeparam name="T"> </typeparam> inside a <see cref="IEnumerable" />.
    /// </summary>
    /// <typeparam name="T">Actual type to look for</typeparam>
    /// <param name="source">Source collection.</param>
    /// <returns>True if there is an element whose type is <typeparam name="T"/></returns>
    public static bool AnyOfType<T>(this IEnumerable source)
    {
        foreach (var obj in source)
        {
            if (obj is T)
            {
                return true;
            }
        }

        return false;
    }
}
