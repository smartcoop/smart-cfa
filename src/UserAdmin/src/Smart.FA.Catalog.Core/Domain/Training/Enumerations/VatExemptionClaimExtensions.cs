using Smart.FA.Catalog.Shared.Domain.Enumerations.Training;

namespace Smart.FA.Catalog.Core.Domain.Enumerations;

public static class VatExemptionClaimExtensions
{
    public static bool IsTrainingAutoValidated(this IEnumerable<VatExemptionClaim> vatExemptionClaims)
        => vatExemptionClaims.Select(vatExemptionClaim => vatExemptionClaim.VatExemptionType).Contains(VatExemptionType.LanguageCourse);
}
