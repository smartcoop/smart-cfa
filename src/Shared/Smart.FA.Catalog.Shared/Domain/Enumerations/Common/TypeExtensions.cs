using System.Reflection;

namespace Smart.FA.Catalog.Shared.Domain.Enumerations.Common;

internal static class TypeExtensions
{
    public static List<TFieldType> GetFieldsOfType<TFieldType>(this Type type)
    {
        return type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(p => type.IsAssignableFrom(p.FieldType))
            .Select(pi => (TFieldType) pi.GetValue(null)!)
            .ToList();
    }
}
