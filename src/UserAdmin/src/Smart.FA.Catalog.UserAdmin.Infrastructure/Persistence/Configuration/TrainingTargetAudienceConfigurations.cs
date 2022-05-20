using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Smart.FA.Catalog.Shared.Domain.Enumerations.Training;
using Smart.FA.Catalog.UserAdmin.Domain.Domain;

namespace Smart.FA.Catalog.UserAdmin.Infrastructure.Persistence.Configuration;

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
            .HasConversion(target => target.Id, id => TargetAudienceType.FromValue(id))
            .HasColumnName($"{nameof(TargetAudienceType)}Id");
    }
}
