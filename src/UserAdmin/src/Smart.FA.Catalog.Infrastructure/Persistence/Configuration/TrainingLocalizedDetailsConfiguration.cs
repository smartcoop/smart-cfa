using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Smart.FA.Catalog.Core.Domain;
using Smart.FA.Catalog.Core.Domain.ValueObjects;

namespace Smart.FA.Catalog.Infrastructure.Persistence.Configuration;

public class TrainingLocalizedDetailsConfiguration : IEntityTypeConfiguration<TrainingLocalizedDetails>
{
    public void Configure(EntityTypeBuilder<TrainingLocalizedDetails> builder)
    {
        builder
            .HasKey(localizedDetails => new {localizedDetails.TrainingId, localizedDetails.Language});
        builder
            .Property(details => details.Title)
            .HasMaxLength(500);
        builder.Property(details => details.Goal)
            .HasMaxLength(4000)
            .IsRequired(false);
        builder.Property(details => details.Methodology)
            .HasMaxLength(4000)
            .IsRequired(false);
        builder.Property(details => details.PracticalModalities)
            .HasMaxLength(4000)
            .IsRequired(false);
        builder.Property(trainer => trainer.Language)
            .HasConversion(language => language.Value,
            language => Language.Create(language).Value).HasColumnName("Language").HasColumnType("NCHAR(2)");
        builder.HasOne(details => details.Training)
            .WithMany(training => training.Details)
            .HasForeignKey(details => details.TrainingId);

        builder.ToTable("TrainingLocalizedDetails");
    }
}
