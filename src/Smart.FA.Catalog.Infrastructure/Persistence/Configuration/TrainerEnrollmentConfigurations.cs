using Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

public class TrainerEnrollmentConfigurations: IEntityTypeConfiguration<TrainerEnrollment>
{
    public void Configure(EntityTypeBuilder<TrainerEnrollment> builder)
    {
        builder.HasKey(enrollment => new {enrollment.TrainingId, enrollment.TrainerId}).IsClustered();

        builder
            .HasOne(enrollment => enrollment.Training)
            .WithMany(training => training.TrainerEnrollments)
            .HasForeignKey(enrollment => enrollment.TrainingId)
            .OnDelete(DeleteBehavior.Cascade);
        builder
            .HasOne(enrollment => enrollment.Trainer)
            .WithMany(enrollment => enrollment.Enrollments)
            .HasForeignKey(enrollment => enrollment.TrainerId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Navigation(enrollment => enrollment.Training);
        builder.Navigation(enrollment => enrollment.Trainer);

        builder.ToTable("TrainerEnrollment");
    }
}
