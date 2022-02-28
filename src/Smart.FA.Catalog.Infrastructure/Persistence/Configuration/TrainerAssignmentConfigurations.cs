using Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

public class TrainerAssignmentConfigurations: IEntityTypeConfiguration<TrainerAssignment>
{
    public void Configure(EntityTypeBuilder<TrainerAssignment> builder)
    {
        builder.HasKey(assignment => new {assignment.TrainingId, assignment.TrainerId}).IsClustered();

        builder
            .HasOne(assignment => assignment.Training)
            .WithMany(training => training.TrainerAssignments)
            .HasForeignKey(assignment => assignment.TrainingId)
            .OnDelete(DeleteBehavior.Cascade);
        builder
            .HasOne(assignment => assignment.Trainer)
            .WithMany(assignment => assignment.Assignments)
            .HasForeignKey(assignment => assignment.TrainerId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Navigation(assignment => assignment.Training);
        builder.Navigation(assignment => assignment.Trainer);

        builder.ToTable("TrainerAssignment");
    }
}
