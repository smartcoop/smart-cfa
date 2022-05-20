namespace Smart.FA.Catalog.UserAdmin.Domain.Domain;

public class TrainerApproval
{
    #region Properties

    public int UserChartId { get; private set; }

    public int TrainerId { get; private set; }

    public virtual UserChartRevision UserChartRevision { get; } = null!;

    public virtual Trainer Trainer { get; } = null!;

    #endregion

    #region Constructors

    protected TrainerApproval()
    {

    }

    public TrainerApproval(Trainer trainer, UserChartRevision userChartRevision)
    {
        Trainer = trainer;
        UserChartRevision = userChartRevision;
        TrainerId = trainer.Id;
        UserChartId = userChartRevision.Id;
    }

    #endregion
}
