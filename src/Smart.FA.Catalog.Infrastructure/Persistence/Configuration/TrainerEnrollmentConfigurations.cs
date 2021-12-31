using Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

public class TrainerEnrollmentConfigurations: IEntityTypeConfiguration<TrainerEnrollment>
{
    public void Configure(EntityTypeBuilder<TrainerEnrollment> builder)
    {
        builder.HasKey(enrollment => new {enrollment.TrainingId, enrollment.TrainerId});

        builder.Ignore(enrollment => enrollment.Trainer);
        builder.Ignore(enrollment => enrollment.Training);

        builder.ToTable("TrainerEnrollment");
    }
}
