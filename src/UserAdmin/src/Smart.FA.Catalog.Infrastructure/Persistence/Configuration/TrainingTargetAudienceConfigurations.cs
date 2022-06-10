using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Smart.FA.Catalog.Core.Domain;
using Smart.FA.Catalog.Shared.Domain.Enumerations.Training;

namespace Smart.FA.Catalog.Infrastructure.Persistence.Configuration;

public class TrainingTargetAudienceConfigurations : IEntityTypeConfiguration<TrainingTargetAudience>
{
    public void Configure(EntityTypeBuilder<TrainingTargetAudience> builder)
    {
        builder.ToTable("TrainingTargetAudience");

        builder.HasKey(target => new { target.TrainingId, target.TargetAudienceType });

        builder.HasOne(targetAudience => targetAudience.Training)
            .WithMany(training => training.Targets)
            .HasForeignKey(targetAudience => targetAudience.TrainingId);

        builder.Property(target => target.TargetAudienceType)
            .HasColumnName($"{nameof(TargetAudienceType)}Id");
    }
}
