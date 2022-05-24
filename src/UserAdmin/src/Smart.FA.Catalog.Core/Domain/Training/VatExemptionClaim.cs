using Smart.FA.Catalog.Shared.Domain.Enumerations.Training;

namespace Smart.FA.Catalog.Core.Domain;

public class VatExemptionClaim
{
    #region Properties

    public int TrainingId { get; set; }
    public virtual VatExemptionType VatExemptionType { get; } = null!;
    public virtual Training Training { get; } = null!;

    #endregion

    #region Constructors

    protected VatExemptionClaim()
    {

    }

    public VatExemptionClaim(Training training, VatExemptionType vatExemptionType)
    {
        Training = training;
        VatExemptionType = vatExemptionType;
        TrainingId = training.Id;
    }

    #endregion
}
