using Core.Domain.Enumerations;

namespace Core.Domain;

public class TrainingIdentity
{
    #region Properties

    public int TrainingId { get; set; }
    public int TrainingTypeId { get; set; }
    public virtual TrainingType TrainingType { get; }
    public virtual Training Training { get; }

    #endregion

    #region Constructors

    protected TrainingIdentity()
    {

    }

    public TrainingIdentity(Training training, TrainingType trainingType)
    {
        Training = training;
        TrainingType = trainingType;
        TrainingId = training.Id;
        TrainingTypeId = trainingType.Id;
    }

    #endregion
}
