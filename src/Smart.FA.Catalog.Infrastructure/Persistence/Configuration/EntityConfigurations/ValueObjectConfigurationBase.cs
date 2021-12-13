using Core.SeedWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration.EntityConfigurations;

public class ValueObjectConfigurationBase<T> : IEntityTypeConfiguration<T>
    where T : ValueObject
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.Property<int>("Id");
        builder.HasKey("Id");
    }
}
