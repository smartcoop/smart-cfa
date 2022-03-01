using Core.Domain;
using Core.Domain.Enumerations;
using Core.SeedWork;
using Infrastructure.Persistence.Configuration.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

internal class TrainingConfiguration : EntityConfigurationBase<Training>
{
    public override void Configure(EntityTypeBuilder<Training> builder)
    {
        base.Configure(builder);
        builder.Property(training => training.Id)
            .ValueGeneratedOnAdd();
        builder.HasMany(training => training.Details)
            .WithOne()
            .HasForeignKey(detail => detail.TrainingId);
        builder.HasMany(training => training.Identities)
            .WithOne()
            .HasForeignKey(identity => identity.TrainingId);
        builder.HasMany(training => training.Targets)
            .WithOne()
            .HasForeignKey(target => target.TrainingId);
        builder.HasMany(training => training.TrainerAssignments)
            .WithOne(assignment => assignment.Training)
            .HasForeignKey(assignment => assignment.TrainingId);
        builder.HasMany(training => training.Slots)
            .WithOne(slot => slot.Training)
            .HasForeignKey(slot => slot.TrainingId);
        builder.HasMany(training => training.Topics)
            .WithOne(category => category.Training)
            .HasForeignKey(category => category.TrainingId);
        builder.Property(training => training.Status)
            .HasConversion(status => status.Id,
            status => Enumeration.FromValue<TrainingStatus>(status))
            .HasColumnName("StatusId")
            .HasColumnType("smallint");

        builder.ToTable("Training");
    }
}
