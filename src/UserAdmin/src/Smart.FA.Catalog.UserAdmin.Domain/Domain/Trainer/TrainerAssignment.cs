namespace Smart.FA.Catalog.UserAdmin.Domain.Domain;

public class TrainerAssignment
{

    #region Properties

    public int TrainingId { get; private set; }
    public int TrainerId { get; private set; }

    public virtual Training Training { get; } = null!;
    public virtual Trainer Trainer { get; } = null!;

    #endregion

    #region Constructors

    protected TrainerAssignment()
    {

    }

    public TrainerAssignment(Training training, Trainer trainer)
    {
        Training = training;
        Trainer = trainer;
        TrainingId = training.Id;
        TrainerId = trainer.Id;
    }

    #endregion

}
