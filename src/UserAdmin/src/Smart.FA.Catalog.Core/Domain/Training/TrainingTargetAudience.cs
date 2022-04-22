using Smart.FA.Catalog.Shared.Domain.Enumerations.Training;

namespace Smart.FA.Catalog.Core.Domain;

public class TrainingTargetAudience
{
    #region Properties

    public int TrainingId { get; }
    public int TargetAudienceTypeId { get; }
    public virtual Training Training { get; } = null!;
    public virtual TargetAudienceType TargetAudienceType { get; } = null!;

    #endregion

    #region Constructors

    protected TrainingTargetAudience()
    {

    }

    public TrainingTargetAudience(Training training, TargetAudienceType targetAudienceType)
    {
        Training = training;
        TargetAudienceType = targetAudienceType;
        TrainingId = training.Id;
        TargetAudienceTypeId = targetAudienceType.Id;
    }

    #endregion
}
