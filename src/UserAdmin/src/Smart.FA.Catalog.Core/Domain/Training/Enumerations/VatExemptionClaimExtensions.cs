namespace Smart.FA.Catalog.Core.Domain.Enumerations;

public static class VatExemptionClaimExtensions
{
    public static bool IsTrainingAutoValidated(this IEnumerable<VatExemptionClaim> vatExemptionClaims)
        => vatExemptionClaims.Any();
}
