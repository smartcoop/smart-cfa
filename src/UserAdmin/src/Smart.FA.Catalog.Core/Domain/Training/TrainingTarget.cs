using Smart.FA.Catalog.Core.Domain.Enumerations;
using Smart.FA.Catalog.Shared.Domain.Enumerations.Training;

namespace Smart.FA.Catalog.Core.Domain;

public class TrainingTarget
{
    #region Properties

    public int TrainingId { get; }
    public int TargetAudienceTypeId { get; }
    public virtual Training Training { get; } = null!;
    public virtual TargetAudienceType TargetAudienceType { get; } = null!;

    #endregion

    #region Constructors

    protected TrainingTarget()
    {

    }

    public TrainingTarget(Training training, TargetAudienceType targetAudienceType)
    {
        Training = training;
        TargetAudienceType = targetAudienceType;
        TrainingId = training.Id;
        TargetAudienceTypeId = targetAudienceType.Id;
    }

    #endregion
}
