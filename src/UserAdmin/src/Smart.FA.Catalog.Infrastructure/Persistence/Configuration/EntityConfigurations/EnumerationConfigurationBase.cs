using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Smart.FA.Catalog.Shared.Domain.Enumerations.Common;

namespace Smart.FA.Catalog.Infrastructure.Persistence.Configuration.EntityConfigurations;

public class EnumerationConfigurationBase<T> : IEntityTypeConfiguration<T>
    where T : Enumeration<T>
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasKey(e => e.Id);

        #region Columns

        builder
           .Property(e => e.Id)
           .ValueGeneratedNever()
           .IsRequired();

        builder
           .Property(e => e.Name)
           .HasMaxLength(255)
           .IsRequired();

        #endregion

        #region Indexes

        builder
           .HasIndex(e => e.Name)
           .IsUnique();

        #endregion

        #region Seeding

        Seed(builder);

        #endregion
    }

    protected virtual void Seed(EntityTypeBuilder<T> builder)
    {
        builder.HasData(Enumeration<T>.List);
    }
}
