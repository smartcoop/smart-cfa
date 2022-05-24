using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Smart.FA.Catalog.Core.Domain;
using Smart.FA.Catalog.Shared.Domain.Enumerations.Training;

namespace Smart.FA.Catalog.Infrastructure.Persistence.Configuration;

public class TrainingTopicConfiguration : IEntityTypeConfiguration<TrainingTopic>
{
    public void Configure(EntityTypeBuilder<TrainingTopic> builder)
    {
        builder.HasKey(trainingTopic => new {trainingTopic.TrainingId, trainingTopic.Topic});
        builder.HasOne(trainingTopic => trainingTopic.Training)
            .WithMany(training => training.Topics)
            .HasForeignKey(trainingTopic => trainingTopic.TrainingId);

        builder.Property(trainingTopic => trainingTopic.Topic)
            .HasConversion(topic => topic.Id, id => Topic.FromValue(id))
            .HasColumnName("TopicId");

        builder.ToTable("TrainingTopic");
    }
}
