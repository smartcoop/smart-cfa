using Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

public class TrainingSlotConfigurations: IEntityTypeConfiguration<TrainingSlot>
{
    public void Configure(EntityTypeBuilder<TrainingSlot> builder)
    {
        builder.HasKey(slot => new {slot.TrainingId, slot.TrainingSlotTypeId});
        builder.HasOne(slot => slot.Training)
            .WithMany(training => training.Slots)
            .HasForeignKey(slot => slot.TrainingId);

        builder.ToTable("TrainingSlot");
    }
}
