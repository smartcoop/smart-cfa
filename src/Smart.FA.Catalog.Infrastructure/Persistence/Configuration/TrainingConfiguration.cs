using Core.Domain;
using Infrastructure.Persistence.Configuration.EntityConfigurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

internal class TrainingConfiguration : EntityConfigurationBase<Training>
{
    public override void Configure(EntityTypeBuilder<Training> builder)
    {
        base.Configure(builder);

        builder.HasKey(builder => builder.Id);
        builder.Property(builder => builder.Name);
    }
}
