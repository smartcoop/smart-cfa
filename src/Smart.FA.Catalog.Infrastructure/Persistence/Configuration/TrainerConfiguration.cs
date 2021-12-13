using Core.Domain;
using Infrastructure.Persistence.Configuration.EntityConfigurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

public class TrainerConfiguration : EntityConfigurationBase<Trainer>
{
    public override void Configure(EntityTypeBuilder<Trainer> builder)
    {
        base.Configure(builder);

        builder.HasKey(builder => builder.Id);
        builder.Property(builder => builder.Name).HasMaxLength(128);
    }
}
