namespace Smart.FA.Catalog.UserAdmin.Domain.Domain.Enumerations;

public static class VatExemptionClaimExtensions
{
    public static bool IsTrainingAutoPublished(this IEnumerable<VatExemptionClaim> vatExemptionClaims)
        => vatExemptionClaims.Any();
}
