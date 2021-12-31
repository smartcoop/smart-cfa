namespace Core.Domain;

public class TrainerEnrollment
{

    #region Properties

    public int TrainingId { get; }
    public int TrainerId { get; }

    public virtual Training Training { get; set; }
    public virtual Trainer Trainer { get; set; }

    #endregion

    #region Constructors

    protected TrainerEnrollment()
    {

    }

    public TrainerEnrollment(Training training, Trainer trainer)
    {
        Training = training;
        Trainer = trainer;
    }

    #endregion

}
