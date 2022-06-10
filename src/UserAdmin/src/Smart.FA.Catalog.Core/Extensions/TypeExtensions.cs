namespace Smart.FA.Catalog.Core.Extensions;

public static class TypeExtensions
{
    /// <summary>
    /// Checks if <paramref name="type"/> is the same as or inherits/implements <paramref name="genericType"/>.
    /// </summary>
    /// <param name="type">The <see cref="Type"/> to check.</param>
    /// <param name="genericType">The generic <see cref="Type"/> definition to check with.</param>
    /// <returns><see langword="true"/> if <paramref name="type"/> is or derives from <paramref name="genericType"/> regardless of whether <paramref name="type"/> is a concrete instantiation of it, otherwise <see langword="false"/>.</returns>
    public static bool DerivesFromGenericType(this Type? type, Type genericType)
    {
        if (type is null)
        {
            return false;
        }

        if (type.GetInterfaces().Any(interfaceType => interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == genericType))
        {
            return true;
        }

        if (type.IsGenericType && type.GetGenericTypeDefinition() == genericType)
        {
            return true;
        }

        return DerivesFromGenericType(type.BaseType, genericType);
    }
}
