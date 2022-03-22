using Smart.FA.Catalog.Core.Domain.Enumerations;

namespace Smart.FA.Catalog.Core.Domain;

public class TrainingTarget
{
    #region Properties

    public int TrainingId { get; }
    public int TrainingTargetAudienceId { get; }
    public virtual Training Training { get; } = null!;
    public virtual TrainingTargetAudience TrainingTargetAudience { get; } = null!;

    #endregion

    #region Constructors

    protected TrainingTarget()
    {

    }

    public TrainingTarget(Training training, TrainingTargetAudience trainingTargetAudience)
    {
        Training = training;
        TrainingTargetAudience = trainingTargetAudience;
        TrainingId = training.Id;
        TrainingTargetAudienceId = trainingTargetAudience.Id;
    }

    #endregion
}
