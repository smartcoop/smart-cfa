namespace Smart.FA.Catalog.Shared.Domain.Enumerations.Common;

public static class EnumerationExtensions
{
    public static bool IsContainedIn<TEnum, TValue>(this Enumeration<TEnum, TValue> enumeration, IEnumerable<TValue>? ids)
        where TEnum : Enumeration<TEnum, TValue>
        where TValue : IEquatable<TValue>, IComparable<TValue>
    {
        return ids?.Contains(enumeration) ?? false;
    }
}
