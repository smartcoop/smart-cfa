using System;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Smart.FA.Catalog.Infrastructure.Persistence.EfCore.Converters;
using Smart.FA.Catalog.Infrastructure.Persistence.EfCore.Extensions;
using Smart.FA.Catalog.Shared.Domain.Enumerations.Common;
using Smart.FA.Catalog.Shared.Domain.Enumerations.Training;
using Xunit;

namespace Smart.FA.Catalog.UnitTests.Infrastructure.EfCore;

public class EnumerationConverterTests
{
    [Fact]
    public void Enumeration_WithOneAsValue_ShouldBeConvertedToOneWhenWrittenToStorage()
    {
        // Arrange
        var converter = new EnumerationConverter<Topic, int>();

        // Act
        var valueWrittenToStorage = converter.ConvertToProvider(Topic.Communication);

        // Assert
        valueWrittenToStorage.Should().BeOfType<int>();
        valueWrittenToStorage.Should().Be(Topic.Communication.Id);
    }

    [Fact]
    public void Enumeration_RetrievedFromStorage_ShouldBeConvertedToEnumeration()
    {
        // Arrange
        var converter = new EnumerationConverter<Topic, int>();

        // Act
        var enumerationFromStorage = converter.ConvertFromProvider(Topic.Communication.Id);

        // Assert
        enumerationFromStorage.Should().BeOfType<Topic>();
        enumerationFromStorage.Should().Be(Topic.Communication);
    }

    [Fact]
    public void Enumeration_RetrievedFromStorage_ShouldNotBeAbleToBeConvertedToAnotherType()
    {
        // Arrange
        var converter = new EnumerationConverter<Topic, int>();

        // Act
        Action act = () => converter.ConvertFromProvider(AttendanceType.Single);

        // Assert
        act.Should().Throw<InvalidCastException>();
    }

    [Fact]
    public void ConverterOnEnumeration_WhenAutomaticConversionIsApplied_ShouldBePresent()
    {
        // Arrange
        var context = new EnumerationContext();
        var fakeEntityProperty = context.Model.FindEntityType(typeof(FakeEntity))!.GetProperty(nameof(FakeEntity.Enum1));

        // Act
        var converter = fakeEntityProperty.GetValueConverter();

        // Arrange
        converter.Should().BeOfType<EnumerationConverter<SomeEnumeration, int>>();
    }
}

public class EnumerationContext : DbContext
{
    public DbSet<FakeEntity> FakeEntities { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConverterOnEnumerations();
    }
}

public class FakeEntity
{
    public int Id { get; set; }

    public SomeEnumeration? Enum1 { get; set; }
}

public class SomeEnumeration : Enumeration<SomeEnumeration>
{
    public SomeEnumeration(int id, string name) : base(id, name)
    {
    }

    public static readonly SomeEnumeration Enum1 = new(1, nameof(Enum1));
}
