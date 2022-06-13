using System.Linq;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Smart.FA.Catalog.Infrastructure.Persistence.EfCore.Extensions;
using Smart.FA.Catalog.Shared.Domain.Enumerations.Common;
using Xunit;

namespace Smart.FA.Catalog.UnitTests.Infrastructure.EfCore;

public class ApplyAutomaticDatabaseColumnNamingOnEnumerations
{
    private static DataContext GetContext()
    {
        var context = new DataContext();
        return context;
    }

    [Fact]
    public void ApplyAutoGenerationEnumerationColumnName_WhenMultipleEnumerationsOfSameTime_ShouldAppendIdToPropertyNames()
    {
        // Arrange
        var context = GetContext();

        // Assert
        var propertyGenre1 = context.Model.FindEntityType(typeof(EntityWithTwoEnumerationOfSameType))!
            .GetProperties()
            .Single(property => property.Name == nameof(EntityWithTwoEnumerationOfSameType.Genre1));

        var databaseGenreId1ColumnName = propertyGenre1.GetColumnName(StoreObjectIdentifier.Table(
            context.EntitiesWithTwoEnumerationOfSameType.EntityType.GetTableName()!,
            context.EntitiesWithTwoEnumerationOfSameType.EntityType.GetSchema()));

        var propertyGenre2 = context.Model.FindEntityType(typeof(EntityWithTwoEnumerationOfSameType))!
            .GetProperties()
            .Single(property => property.Name == nameof(EntityWithTwoEnumerationOfSameType.Genre2));

        var databaseGenreId2ColumnName = propertyGenre2.GetColumnName(StoreObjectIdentifier.Table(
            context.EntitiesWithTwoEnumerationOfSameType.EntityType.GetTableName()!,
            context.EntitiesWithTwoEnumerationOfSameType.EntityType.GetSchema()));

        databaseGenreId1ColumnName.Should().Be("Genre1Id");
        databaseGenreId2ColumnName.Should().Be("Genre2Id");
    }

    [Fact]
    public void ApplyAutoGenerationEnumerationColumnName_WhenOnlyOneEnumerationOfTypeT_ColumnNameShouldBeTypeNameAppendedWithIdWord()
    {
        // Arrange
        var context = GetContext();

        // Assert
        var propertyGenre1 = context.Model.FindEntityType(typeof(EntityWithOneEnumeration))!
            .GetProperties()
            .First(property => property.Name == nameof(EntityWithOneEnumeration.Genre));

        var columnName = propertyGenre1.GetColumnName(StoreObjectIdentifier.Table(
            context.EntitiesWithOneEnumeration.EntityType.GetTableName()!,
            context.EntitiesWithOneEnumeration.EntityType.GetSchema()));

        columnName.Should().Be("BookGenreId");
    }

    private sealed class BookGenre : Enumeration<BookGenre>
    {
        public static readonly BookGenre Anthology = new(1, nameof(Anthology));
        public static readonly BookGenre Biography = new(2, nameof(Biography));
        public static readonly BookGenre SciFi = new(1, nameof(SciFi));

        private BookGenre(int id, string name) : base(id, name)
        {
        }
    }

    private sealed class EntityWithOneEnumeration
    {
        public int Id { get; set; }

        public BookGenre Genre { get; set; } = null!;
    }

    private sealed class EntityWithTwoEnumerationOfSameType
    {
        public int Id { get; set; }

        public BookGenre Genre1 { get; set; } = null!;

        public BookGenre Genre2 { get; set; } = null!;
    }

    private sealed class DataContext : DbContext
    {
        public DbSet<EntityWithTwoEnumerationOfSameType> EntitiesWithTwoEnumerationOfSameType { get; set; } = null!;

        public DbSet<EntityWithOneEnumeration> EntitiesWithOneEnumeration { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlServer("1234");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConverterOnEnumerations();
            modelBuilder.ApplyAutomaticDatabaseColumnNamingOnEnumerations();
        }
    }
}
