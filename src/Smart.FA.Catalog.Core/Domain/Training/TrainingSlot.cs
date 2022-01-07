namespace Core.Domain;

public class TrainingSlot
{
    #region Properties

    public int TrainingId { get; private set; }
    public int TrainingSlotTypeId { get; private set; }
    public virtual TrainingSlotNumberType TrainingSlotNumberSlotType { get; }
    public virtual Training Training { get; }

    #endregion

    #region Constructors

    protected TrainingSlot()
    {

    }

    public TrainingSlot(Training training, TrainingSlotNumberType trainingSlotNumberType)
    {
        Training = training;
        TrainingSlotNumberSlotType = trainingSlotNumberType;
        TrainingId = training.Id;
        TrainingSlotTypeId = trainingSlotNumberType.Id;
    }

    #endregion
}
