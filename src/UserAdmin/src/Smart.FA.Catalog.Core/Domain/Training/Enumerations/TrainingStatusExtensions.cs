using Smart.FA.Catalog.Shared.Domain.Enumerations.Training;

namespace Smart.FA.Catalog.Core.Domain.Enumerations;

public static class TrainingStatusExtensions
{
    public static TrainingStatus Validate(this TrainingStatus status, IEnumerable<VatExemptionClaim> vatExemptionClaims)
        => vatExemptionClaims.IsTrainingAutoValidated() ? TrainingStatus.Validated : TrainingStatus.WaitingForValidation;
}
