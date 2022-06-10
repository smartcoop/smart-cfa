using System.Reflection;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Smart.FA.Catalog.Core.Extensions;
using Smart.FA.Catalog.Shared.Domain.Enumerations.Common;

namespace Smart.FA.Catalog.Infrastructure.Persistence.EfCore.Converters;

/// <summary>
/// Converts a <see cref="Enumeration{TEnum}" /> from its type to <see cref="Enumeration{TEnum,TId}.Id"/> and vice versa.
/// </summary>
/// <typeparam name="TEnum">The type of the enumeration to convert.</typeparam>
/// <typeparam name="TId">The type of the enumeration id.</typeparam>
public class EnumerationConverter<TEnum, TId> : ValueConverter<TEnum, TId>
    where TEnum : Enumeration<TEnum, TId>
    where TId : IEquatable<TId>, IComparable<TId>
{
    private static bool CanConvert(Type objectType)
    {
        return objectType.DerivesFromGenericType(typeof(Enumeration<,>));
    }

    private static MethodInfo? GetBaseFromValueMethod(Type objectType, Type valueType)
    {
        var currentType = objectType.BaseType;

        while (currentType is not null && currentType != typeof(object))
        {
            if (currentType.IsGenericType && currentType.GetGenericTypeDefinition() == typeof(Enumeration<,>))
            {
                return currentType.GetMethod(nameof(Enumeration<TEnum, TId>.FromValue), new Type[] { valueType })!;
            }

            currentType = currentType.BaseType;
        }

        return null;
    }

    private static TEnum? GetFromValue(TId value)
    {
        if (!CanConvert(typeof(TEnum)))
        {
            throw new NotImplementedException();
        }

        var method = GetBaseFromValueMethod(typeof(TEnum), typeof(int));

        var retValue = method?.Invoke(null, new[] { (object)value }) as TEnum;

        return retValue;
    }

    public EnumerationConverter() : base(
        item => item.Id,
        key => GetFromValue(key)!)
    {
    }
}
