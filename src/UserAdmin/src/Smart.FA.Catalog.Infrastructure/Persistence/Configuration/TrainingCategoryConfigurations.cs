using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Smart.FA.Catalog.Core.Domain;

namespace Smart.FA.Catalog.Infrastructure.Persistence.Configuration;

public class TrainingCategoryConfigurations: IEntityTypeConfiguration<TrainingCategory>
{
    public void Configure(EntityTypeBuilder<TrainingCategory> builder)
    {
        builder.HasKey(category => new {category.TrainingId, category.TopicId});
        builder.HasOne(category => category.Training)
            .WithMany(training => training.Topics)
            .HasForeignKey(slot => slot.TrainingId);

        builder.ToTable("TrainingCategory");
    }
}
