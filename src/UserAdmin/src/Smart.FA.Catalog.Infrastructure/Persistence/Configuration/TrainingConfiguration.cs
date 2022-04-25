using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Smart.FA.Catalog.Core.Domain;
using Smart.FA.Catalog.Core.Domain.Enumerations;
using Smart.FA.Catalog.Core.SeedWork;
using Smart.FA.Catalog.Infrastructure.Persistence.Configuration.EntityConfigurations;
using Smart.FA.Catalog.Shared.Domain.Enumerations;
using Smart.FA.Catalog.Shared.Domain.Enumerations.Training;

namespace Smart.FA.Catalog.Infrastructure.Persistence.Configuration;

internal class TrainingConfiguration : EntityConfigurationBase<Training>
{
    public override void Configure(EntityTypeBuilder<Training> builder)
    {
        base.Configure(builder);
        builder.Property(training => training.Id)
            .ValueGeneratedOnAdd();
        builder.HasMany(training => training.Details)
            .WithOne()
            .HasForeignKey(localizedDetails => localizedDetails.TrainingId);
        builder.HasMany(training => training.VatExemptionClaims)
            .WithOne()
            .HasForeignKey(vatExemptionClaim => vatExemptionClaim.TrainingId);
        builder.HasMany(training => training.Targets)
            .WithOne()
            .HasForeignKey(target => target.TrainingId);
        builder.HasMany(training => training.TrainerAssignments)
            .WithOne(assignment => assignment.Training)
            .HasForeignKey(assignment => assignment.TrainingId);
        builder.HasMany(training => training.Attendances)
            .WithOne(attendance => attendance.Training)
            .HasForeignKey(attendance => attendance.TrainingId);
        builder.HasMany(training => training.Topics)
            .WithOne(trainingTopic => trainingTopic.Training)
            .HasForeignKey(trainingTopic => trainingTopic.TrainingId);
        builder.Property(training => training.StatusType)
            .HasConversion(status => status.Id,
            status => Enumeration.FromValue<TrainingStatusType>(status))
            .HasColumnName("TrainingStatusTypeId");

        builder.ToTable("Training");
    }
}
