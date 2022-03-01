using Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

public class TrainingCategoryConfigurations: IEntityTypeConfiguration<TrainingCategory>
{
    public void Configure(EntityTypeBuilder<TrainingCategory> builder)
    {
        builder.HasKey(category => new {category.TrainingId, category.TrainingTopicId});
        builder.HasOne(category => category.Training)
            .WithMany(training => training.Topics)
            .HasForeignKey(slot => slot.TrainingId);

        builder.ToTable("TrainingCategory");
    }
}
