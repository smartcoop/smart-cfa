using Smart.FA.Catalog.Core.Domain.Enumerations;

namespace Smart.FA.Catalog.Core.Domain;

public class TrainingCategory
{
    #region Properties

    public int TrainingId { get; private set; }
    public int TrainingTopicId { get; private set; }
    public virtual TrainingTopic TrainingTopic { get; } = null!;
    public virtual Training Training { get; } = null!;

    #endregion

    #region Constructors

    protected TrainingCategory()
    {

    }

    public TrainingCategory(Training training, TrainingTopic trainingTopic)
    {
        Training = training;
        TrainingTopic = trainingTopic;
        TrainingId = training.Id;
        TrainingTopicId = trainingTopic.Id;
    }

    #endregion
}
