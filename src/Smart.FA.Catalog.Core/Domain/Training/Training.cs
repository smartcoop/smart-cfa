using Core.Domain.Enumerations;
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
    private readonly List<TrainingSlot> _slots = new();

    #endregion

    #region Properties

    public virtual IReadOnlyCollection<TrainerEnrollment> TrainerEnrollments => _trainerEnrollments;
    public virtual IReadOnlyCollection<TrainingIdentity> Identities => _Identities;
    public virtual IReadOnlyCollection<TrainingTarget> Targets => _targets;
    public virtual IReadOnlyCollection<TrainingDetail> Details => _details;
    public virtual IReadOnlyCollection<TrainingSlot> Slots => _slots;

    public int TrainerCreatorId { get; }
    public int StatusId { get; private set; } = TrainingStatus.Draft.Id;

    #endregion

    #region Constructors

    public Training(Trainer trainer, IEnumerable<TrainingType> types,
        IEnumerable<TrainingSlotNumberType> slotNumberTypes,
        IEnumerable<TrainingTargetAudience> targetAudiences)
    {
        SwitchTrainingTypes(types);
        SwitchTargetAudience(targetAudiences);
        SwitchSlotNumberType(slotNumberTypes);
        EnrollTrainer(trainer);
        TrainerCreatorId = trainer.Id;
    }

    protected Training()
    {
    }

    #endregion

    #region Public methods

    public void SwitchTrainingTypes(IEnumerable<TrainingType> trainingTypes)
    {
        Guard.Requires(() => trainingTypes != null, "training types should not be null");
        _Identities.Clear();
        _Identities.AddRange(trainingTypes.Distinct().Select(trainingType => new TrainingIdentity(this, trainingType)));
    }

    public void SwitchTargetAudience(IEnumerable<TrainingTargetAudience> trainingTargetAudiences)
    {
        Guard.Requires(() => trainingTargetAudiences != null, "training audiences should not be null");
        _targets.Clear();
        _targets.AddRange(trainingTargetAudiences.Distinct()
            .Select(trainingAudience => new TrainingTarget(this, trainingAudience)));
    }

    public void SwitchSlotNumberType(IEnumerable<TrainingSlotNumberType> slotNumberTypes)
    {
        Guard.Requires(() => slotNumberTypes != null, "slot number types should not be null");
        _slots.Clear();
        _slots.AddRange(slotNumberTypes.Distinct()
            .Select(slotNumberType => new TrainingSlot(this, slotNumberType)));
    }

    public void EnrollTrainer(Trainer trainer)
    {
        Guard.Requires(() => trainer != null, "There should be at least one trainer enrolled (owner)");
        TrainerEnrollment trainerEnrollment = new(this, trainer);
        if (_trainerEnrollments.Contains(trainerEnrollment)) throw new Exception();
        _trainerEnrollments.Add(trainerEnrollment);
    }

    public void EnrollTrainers(IEnumerable<Trainer> trainers)
    {
        DisenrollAll();
        foreach (var trainer in trainers)
        {
            EnrollTrainer(trainer);
        }
    }

    public void DisenrollAll()
        => _trainerEnrollments.RemoveAll(enrollment => enrollment.TrainerId != TrainerCreatorId);

    public bool Validate(IMailService mailService)
    {
        if (MissingFields().Any())
        {
            StatusId = TrainingStatus.Draft.Id;
            return false;
        }

        var status = Enumeration.FromValue<TrainingStatus>(StatusId);
        StatusId = status.Validate(_Identities.Select(identity => identity.TrainingType)).Id;

        return true;
    }

    public void AddDetails(string title, string goal, string methodology, Language language)
    {
        Guard.Requires(() => _details.FirstOrDefault(detail => detail.Language == language.Value) == null,
            "A description for that language already exists");
        _details.Add(new TrainingDetail(this, title, goal, methodology, language.Value));
    }

    public void UpdateDetails(string title, string goal, string methodology, Language language)
    {
        Guard.Requires(() => _details.FirstOrDefault(detail => detail.Language == language.Value) != null,
            "No descriptions for that language exist");
       var detailToModify = _details.First(detail => detail.Language == language.Value);
       detailToModify.Goal = goal;
       detailToModify.Title = title;
       detailToModify.Methodology = methodology;
    }
    #endregion

    #region Private methods

    private IEnumerable<string> MissingFields()
    {
        List<string> errors = new();
        if (!Identities.Any()) errors.Add("Missing Identities");
        if (!Targets.Any()) errors.Add("Missing Targets");
        if (!TrainerEnrollments.Any()) errors.Add("Missing Trainer enrollments");
        return errors;
    }

    #endregion
}
