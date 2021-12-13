using Core.SeedWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration.EntityConfigurations;

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
           .IsRequired();

        builder
           .Property(e => e.LastModifiedAt)
           .IsRequired();

        builder.Ignore(e => e.DomainEvents);
    }
}
