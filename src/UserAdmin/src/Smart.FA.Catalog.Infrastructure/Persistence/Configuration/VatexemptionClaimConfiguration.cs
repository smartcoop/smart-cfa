using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Smart.FA.Catalog.Core.Domain;
using Smart.FA.Catalog.Shared.Domain.Enumerations.Training;

namespace Smart.FA.Catalog.Infrastructure.Persistence.Configuration;

public class VatexemptionClaimConfiguration: IEntityTypeConfiguration<VatExemptionClaim>
{
    public void Configure(EntityTypeBuilder<VatExemptionClaim> builder)
    {
        builder.HasKey(vatExemptionClaim => new {vatExemptionClaim.TrainingId, vatExemptionClaim.VatExemptionType});
        builder.HasOne(vatExemptionClaim => vatExemptionClaim.Training)
            .WithMany(training => training.VatExemptionClaims)
            .HasForeignKey(identity => identity.TrainingId);


        builder.Property(e => e.VatExemptionType)
            .HasConversion(e => e.Id, id => VatExemptionType.FromValue(id))
            .HasColumnName($"{nameof(VatExemptionType)}Id");

        builder.ToTable("VatExemptionClaim");
    }
}
