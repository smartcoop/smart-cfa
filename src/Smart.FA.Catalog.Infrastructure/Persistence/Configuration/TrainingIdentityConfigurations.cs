using Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

public class TrainingIdentityConfigurations: IEntityTypeConfiguration<TrainingIdentity>
{
    public void Configure(EntityTypeBuilder<TrainingIdentity> builder)
    {
        builder.HasKey(enrollment => new {enrollment.TrainingId, enrollment.TrainingTypeId});
        builder.HasOne(enrollment => enrollment.Training).WithMany(training => training.Identities)
            .HasForeignKey(enrollment => enrollment.TrainingId);

        builder.ToTable("TrainingIdentity");
    }
}
