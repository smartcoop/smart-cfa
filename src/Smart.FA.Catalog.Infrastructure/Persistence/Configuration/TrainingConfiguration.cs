using Core.Domain;
using Infrastructure.Persistence.Configuration.EntityConfigurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

internal class TrainingConfiguration : EntityConfigurationBase<Training>
{
    public override void Configure(EntityTypeBuilder<Training> builder)
    {
        base.Configure(builder);

        builder.HasMany(training => training.Details).WithOne().HasForeignKey(detail => detail.TrainingId);
        builder.HasMany(training => training.Identities).WithOne().HasForeignKey(identity => identity.TrainingId);
        builder.HasMany(training => training.Targets).WithOne().HasForeignKey(target => target.TrainingId);
        builder.HasMany(training => training.TrainerEnrollments).WithOne()
            .HasForeignKey(enrollment => enrollment.TrainingId);

    }
}
