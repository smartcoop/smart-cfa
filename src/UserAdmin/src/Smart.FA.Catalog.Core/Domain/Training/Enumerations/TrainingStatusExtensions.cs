using Smart.FA.Catalog.Shared.Domain.Enumerations.Training;

namespace Smart.FA.Catalog.Core.Domain.Enumerations;

public static class TrainingStatusTypeExtensions
{
    public static TrainingStatusType Validate(this TrainingStatusType statusType, IEnumerable<VatExemptionClaim> vatExemptionClaims)
        => vatExemptionClaims.IsTrainingAutoValidated() ? TrainingStatusType.Validated : TrainingStatusType.WaitingForValidation;
}
