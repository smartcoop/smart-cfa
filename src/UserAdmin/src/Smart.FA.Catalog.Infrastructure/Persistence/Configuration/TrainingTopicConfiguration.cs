using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Smart.FA.Catalog.Core.Domain;

namespace Smart.FA.Catalog.Infrastructure.Persistence.Configuration;

public class TrainingTopicConfiguration : IEntityTypeConfiguration<TrainingTopic>
{
    public void Configure(EntityTypeBuilder<TrainingTopic> builder)
    {
        builder.HasKey(trainingTopic => new {trainingTopic.TrainingId, trainingTopic.TopicId});
        builder.HasOne(trainingTopic => trainingTopic.Training)
            .WithMany(training => training.Topics)
            .HasForeignKey(trainingTopic => trainingTopic.TrainingId);

        builder.ToTable("TrainingTopic");
    }
}
