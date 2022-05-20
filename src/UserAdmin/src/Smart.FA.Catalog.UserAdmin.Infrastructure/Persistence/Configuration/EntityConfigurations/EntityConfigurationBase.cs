using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Smart.FA.Catalog.UserAdmin.Domain.SeedWork;

namespace Smart.FA.Catalog.UserAdmin.Infrastructure.Persistence.Configuration.EntityConfigurations;

public class EntityConfigurationBase<T> : IEntityTypeConfiguration<T>
    where T : Entity
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasKey(e => e.Id);

        builder
           .Ignore(e => e.IsTransient);

        builder
           .Property(e => e.CreatedAt)
           .HasPrecision(3)
           .IsRequired();

        builder
           .Property(e => e.LastModifiedAt)
           .HasPrecision(3)
           .IsRequired();

        builder.Ignore(e => e.DomainEvents);
    }
}
