using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Smart.FA.Catalog.Core.Domain;

namespace Smart.FA.Catalog.Infrastructure.Persistence.Configuration;

public class TrainingTargetAudienceConfigurations : IEntityTypeConfiguration<TrainingTargetAudience>
{
    public void Configure(EntityTypeBuilder<TrainingTargetAudience> builder)
    {
        builder.ToTable("TrainingTargetAudience");

        builder.HasKey(target => new { target.TrainingId, target.TargetAudienceTypeId });

        builder.HasOne(targetAudience => targetAudience.Training)
            .WithMany(training => training.Targets)
            .HasForeignKey(targetAudience => targetAudience.TrainingId);
    }
}
