using Core.SeedWork;
using Core.Services;

namespace Core.Domain;

public class Training : Entity, IAggregateRoot
{
    #region Private fields

    private readonly List<TrainerEnrollment> _trainerEnrollments = new();
    private readonly List<TrainingIdentity> _Identities = new();
    private readonly List<TrainingTarget> _targets = new();
    private readonly List<TrainingDetail> _details = new();
    private TrainingStatus _status = TrainingStatus.Draft;

    #endregion

    #region Properties

    public virtual IReadOnlyCollection<TrainerEnrollment> TrainerEnrollments => _trainerEnrollments;
    public virtual IReadOnlyCollection<TrainingIdentity> Identities => _Identities;
    public virtual IReadOnlyCollection<TrainingTarget> Targets => _targets;

    public virtual TrainingStatus Status => _status;

    public virtual TrainingSlotNumberType SlotNumberType { get; private set; }
    public virtual IReadOnlyCollection<TrainingDetail> Details => _details;

    #endregion

    #region Constructors

    public Training(Trainer trainer, IEnumerable<TrainingType> types, TrainingSlotNumberType slotNumberType,
        IEnumerable<TrainingTargetAudience> targetAudiences)
    {
        if (types == null || !types.Any()) throw new Exception();
        if (targetAudiences == null || !targetAudiences.Distinct().Any()) throw new Exception();
        if (trainer == null) throw new Exception();

        SwitchTrainingTypes(types);
        SwitchTargetAudience(targetAudiences);
        EnrollTrainer(trainer);
    }

    protected Training()
    {

    }

    #endregion
    public void SwitchTrainingTypes(IEnumerable<TrainingType> trainingTypes)
    {
        _Identities.Clear();
        _Identities.AddRange(trainingTypes.Distinct().Select(trainingType => new TrainingIdentity(this, trainingType)));
    }

    public void SwitchTargetAudience(IEnumerable<TrainingTargetAudience> trainingTargetAudiences)
    {
        _targets.Clear();
        _targets.AddRange(trainingTargetAudiences.Distinct()
            .Select(trainingAudience => new TrainingTarget(this, trainingAudience)));
    }

    public void SwitchSlotNumberType(TrainingSlotNumberType slotNumberType)
        =>        SlotNumberType = slotNumberType;

    public void EnrollTrainer(Trainer trainer)
    {
        TrainerEnrollment trainerEnrollment = new(this, trainer);
        if (_trainerEnrollments.Contains(trainerEnrollment)) throw new Exception();
        _trainerEnrollments.Add(trainerEnrollment);
    }

    public void Validate(IMailService mailService)
    {
        _status = _status.Validate(_Identities.Select(identity => identity.TrainingType));
    }

    public void AddDetails(string title, string goal, string methodology, string language)
    {
        if (_details.FirstOrDefault(detail => detail.Language == language) != null) throw new Exception();
        _details.Add(new TrainingDetail(this, title, goal, methodology, language));
    }

    public void UpdateDetails(string title, string goal, string methodology, string language)
    {
        var trainingDetail = _details.FirstOrDefault(detail => detail.Language == language);
        if (trainingDetail == null) throw new Exception();
        trainingDetail.UpdateDescription(title, goal, methodology);
    }

}
