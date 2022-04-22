using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Smart.FA.Catalog.Core.Domain;

namespace Smart.FA.Catalog.Infrastructure.Persistence.Configuration;

public class VatexemptionClaimConfiguration: IEntityTypeConfiguration<VatExemptionClaim>
{
    public void Configure(EntityTypeBuilder<VatExemptionClaim> builder)
    {
        builder.HasKey(vatExemptionClaim => new {vatExemptionClaim.TrainingId, vatExemptionClaim.VatExemptionTypeId});
        builder.HasOne(vatExemptionClaim => vatExemptionClaim.Training)
            .WithMany(training => training.VatExemptionClaims)
            .HasForeignKey(identity => identity.TrainingId);

        builder.ToTable("VatExemptionClaim");
    }
}
