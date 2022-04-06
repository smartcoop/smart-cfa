using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Smart.FA.Catalog.Core.Domain;
using Smart.FA.Catalog.Core.Domain.ValueObjects;

namespace Smart.FA.Catalog.Infrastructure.Persistence.Configuration;

public class TrainingDetailConfigurations : IEntityTypeConfiguration<TrainingDetail>
{
    public void Configure(EntityTypeBuilder<TrainingDetail> builder)
    {
        builder
            .HasKey(detail => new {detail.TrainingId, detail.Language});
        builder
            .Property(detail => detail.Title)
            .HasMaxLength(500);
        builder.Property(detail => detail.Goal)
            .HasMaxLength(1500)
            .IsRequired(false);
        builder.Property(detail => detail.Methodology)
            .HasMaxLength(1500)
            .IsRequired(false);
        builder.Property(detail => detail.PracticalModalities)
            .HasMaxLength(1500)
            .IsRequired(false);
        builder.Property(trainer => trainer.Language)
            .HasConversion(language => language.Value,
            language => Language.Create(language).Value).HasColumnName("Language").HasColumnType("NCHAR(2)");
        builder.HasOne(detail => detail.Training)
            .WithMany(training => training.Details)
            .HasForeignKey(detail => detail.TrainingId);

        builder.ToTable("TrainingDetail");
    }
}
