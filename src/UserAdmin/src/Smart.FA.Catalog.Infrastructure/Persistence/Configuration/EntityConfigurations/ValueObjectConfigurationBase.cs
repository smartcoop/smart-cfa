using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Smart.FA.Catalog.Core.SeedWork;

namespace Smart.FA.Catalog.Infrastructure.Persistence.Configuration.EntityConfigurations;

public class ValueObjectConfigurationBase<T> : IEntityTypeConfiguration<T>
    where T : ValueObject
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.Property<int>("Id");
        builder.HasKey("Id");
    }
}
