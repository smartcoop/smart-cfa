using Smart.FA.Catalog.Core.Domain.Enumerations;
using Smart.FA.Catalog.Shared.Domain.Enumerations.Training;

namespace Smart.FA.Catalog.Core.Domain;

public class TrainingCategory
{
    #region Properties

    public int TrainingId { get; private set; }
    public int TopicId { get; private set; }
    public virtual Topic Topic { get; } = null!;
    public virtual Training Training { get; } = null!;

    #endregion

    #region Constructors

    protected TrainingCategory()
    {

    }

    public TrainingCategory(Training training, Topic topic)
    {
        Training = training;
        Topic = topic;
        TrainingId = training.Id;
        TopicId = topic.Id;
    }

    #endregion
}
