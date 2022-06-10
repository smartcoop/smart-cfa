using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Smart.FA.Catalog.Core.Extensions;
using Smart.FA.Catalog.Infrastructure.Persistence.EfCore.Converters;
using Smart.FA.Catalog.Shared.Domain.Enumerations.Common;

namespace Smart.FA.Catalog.Infrastructure.Persistence.EfCore.Extensions;

public static class ModelBuilderExtensions
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
}
