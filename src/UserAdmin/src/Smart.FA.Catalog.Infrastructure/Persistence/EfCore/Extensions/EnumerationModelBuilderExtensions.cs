using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Smart.FA.Catalog.Core.Extensions;
using Smart.FA.Catalog.Infrastructure.Persistence.EfCore.Converters;
using Smart.FA.Catalog.Shared.Domain.Enumerations.Common;

namespace Smart.FA.Catalog.Infrastructure.Persistence.EfCore.Extensions;

public static class EnumerationModelBuilderExtensions
{
    /// <summary>
    /// Applies automatic conversion between enumeration values and their type.
    /// </summary>
    /// <param name="modelBuilder">The <see cref="ModelBuilder" /> on which is applied the automatic conversion.</param>
    public static void ApplyConverterOnEnumerations(this ModelBuilder modelBuilder)
    {
        var entityTypes = modelBuilder.Model.GetEntityTypes();

        foreach (var entityType in entityTypes)
        {
            foreach (var mutableProperty in entityType.ClrType.GetProperties()
                         .Where(propertyInfo => propertyInfo.PropertyType.DerivesFromGenericType(typeof(Enumeration<,>))))
            {
                // Enable converter for enumeration.
                var valueType = mutableProperty.PropertyType.GetEnumerationValueType();
                var converterType = typeof(EnumerationConverter<,>).MakeGenericType(mutableProperty.PropertyType, valueType!);
                var converter = (ValueConverter)Activator.CreateInstance(converterType)!;
                modelBuilder.Entity(entityType.Name).Property(mutableProperty.Name).HasConversion(converter);
            }
        }
    }

    /// <summary>
    /// Applies to every entities derived from <see cref="Enumeration{TEnum, TId}"/> generic naming of their column.
    /// </summary>
    /// <param name="modelBuilder"></param>
    public static void ApplyAutomaticDatabaseColumnNamingOnEnumerations(this ModelBuilder modelBuilder)
    {
        var entities = modelBuilder.Model.GetEntityTypes();

        foreach (var mutableEntityType in entities)
        {
            var enumerationPropertiesGroupedByType = mutableEntityType.GetPropertiesThatAreEnumerations();

            foreach (var mutableProperty in enumerationPropertiesGroupedByType)
            {
                if (mutableProperty.Count() == 1)
                {
                    // Make the enumeration column name equal to the Property name suffixed with `Id`.
                    mutableProperty.First().SetColumnName(mutableProperty.First().ClrType.Name + "Id");
                }
                else
                {
                    // Name will be [PropertyName]Id
                    mutableProperty.ApplyConfigurationWhenMultipleEnumerationsOfSameType();
                }
            }
        }
    }

    internal static ILookup<Type, IMutableProperty> GetPropertiesThatAreEnumerations(this IMutableEntityType mutableEntityType)
    {
        var enumerationPropertiesGroupedByType = mutableEntityType
            .GetProperties()
            .Where(mutableProperty => mutableProperty.ClrType.DerivesFromGenericType(typeof(Enumeration<,>)))
            .ToLookup(mutableProperty => mutableProperty.ClrType, mutableProperty => mutableProperty);
        return enumerationPropertiesGroupedByType;
    }

    internal static void ApplyConfigurationWhenMultipleEnumerationsOfSameType(this IGrouping<Type, IMutableProperty> mutableProperty)
    {
        // When having multiple properties of the same type, we can't just add a suffix "Id", it will cause duplication of column names.
        foreach (var property in mutableProperty)
        {
            property.SetColumnName($"{property.Name}Id");
        }
    }
}
