using Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

public class TrainingSlotConfigurations: IEntityTypeConfiguration<TrainingSlot>
{
    public void Configure(EntityTypeBuilder<TrainingSlot> builder)
    {
        builder.HasKey(enrollment => new {enrollment.TrainingId, enrollment.TrainingSlotTypeId});
        builder.HasOne(enrollment => enrollment.Training).WithMany(training => training.Slots)
            .HasForeignKey(enrollment => enrollment.TrainingId);

        builder.ToTable("TrainingSlot");
    }
}
