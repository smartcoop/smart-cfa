using Smart.FA.Catalog.Shared.Domain.Enumerations.Common;

namespace Smart.FA.Catalog.Infrastructure.Persistence.EfCore.Extensions;

internal static class TypeExtensions
{
    internal static Type? GetEnumerationValueType(this Type objectType)
    {
        var currentType = objectType.BaseType;

        if (currentType is null)
        {
            return null;
        }

        while (currentType != typeof(object))
        {
            if (currentType.IsGenericType && currentType.GetGenericTypeDefinition() == typeof(Enumeration<,>))
            {
                return currentType.GenericTypeArguments[1];
            }

            currentType = currentType.BaseType!;
        }

        return null;
    }
}
