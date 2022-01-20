using System.Linq.Expressions;
using Core.Domain.Dto;
using Core.Domain.Enumerations;
using Core.SeedWork;
using Core.Services;

namespace Core.Domain;

public class Training : Entity, IAggregateRoot
{
    #region Private fields

    private readonly List<TrainerEnrollment> _trainerEnrollments = new();
    private readonly List<TrainingIdentity> _identities = new();
    private readonly List<TrainingTarget> _targets = new();
    private readonly List<TrainingDetail> _details = new();
    private readonly List<TrainingSlot> _slots = new();

    #endregion

    #region Properties

    public virtual IReadOnlyCollection<TrainerEnrollment> TrainerEnrollments => _trainerEnrollments;
    public virtual IReadOnlyCollection<TrainingIdentity> Identities => _identities;
    public virtual IReadOnlyCollection<TrainingTarget> Targets => _targets;
    public virtual IReadOnlyCollection<TrainingDetail> Details => _details;
    public virtual IReadOnlyCollection<TrainingSlot> Slots => _slots;

    public int TrainerCreatorId { get; }
    public int StatusId { get; private set; } = TrainingStatus.Draft.Id;

    #endregion

    #region Constructors

    public Training(Trainer trainer, TrainingDetailDto trainingDetail, IEnumerable<TrainingType> types,
        IEnumerable<TrainingSlotNumberType> slotNumberTypes,
        IEnumerable<TrainingTargetAudience> targetAudiences)
    {
        AddDetails(trainingDetail.Title, trainingDetail.Goal, trainingDetail.Methodology,
            Language.Create(trainingDetail.Language).Value);
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

    public void SwitchTrainingTypes(IEnumerable<TrainingType>? trainingTypes)
    {
        Guard.Requires(() => trainingTypes != null, "training types should not be null");
        _identities.Clear();
        _identities.AddRange(trainingTypes!.Distinct()
            .Select(trainingType => new TrainingIdentity(this, trainingType)));
    }

    public void SwitchTargetAudience(IEnumerable<TrainingTargetAudience>? trainingTargetAudiences)
    {
        Guard.Requires(() => trainingTargetAudiences != null, "training audiences should not be null");
        _targets.Clear();
        _targets.AddRange(trainingTargetAudiences!.Distinct()
            .Select(trainingAudience => new TrainingTarget(this, trainingAudience)));
    }

    public void SwitchSlotNumberType(IEnumerable<TrainingSlotNumberType>? slotNumberTypes)
    {
        Guard.Requires(() => slotNumberTypes != null, "slot number types should not be null");
        _slots.Clear();
        _slots.AddRange(slotNumberTypes!.Distinct()
            .Select(slotNumberType => new TrainingSlot(this, slotNumberType)));
    }

    public void EnrollTrainer(Trainer? trainer)
    {
        Guard.Requires(() => trainer != null, "There should be at least one trainer enrolled (owner)");
        TrainerEnrollment trainerEnrollment = new(this, trainer!);
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

    public List<string> Validate()
    {
        var missingFieldsErrors = ListMissingFields();

        StatusId = missingFieldsErrors.Any()
            ? TrainingStatus.Draft.Id
            : ValidateStatus.Compile()(this).Id;

        var trainingDetail = Details.FirstOrDefault(training => training.Language == Language.Create("EN").Value) ?? Details.First();
        AddDomainEvent(new ValidateTrainingEvent(trainingDetail.Title!, Id, TrainerEnrollments.Select(enrollment => enrollment.TrainerId)));

        return missingFieldsErrors;
    }

    public void AddDetails(string? title, string? goal, string? methodology, Language language)
    {
        Guard.Requires(() => _details.FirstOrDefault(detail => detail.Language.Value == language.Value) == null,
            "A description for that language already exists");
        _details.Add(new TrainingDetail(this, title, goal, methodology, language));
    }

    public void UpdateDetails(string title, string goal, string methodology, Language language)
    {
        var detailToModify = _details.FirstOrDefault(detail => detail.Language.Value == language.Value);
        Guard.Requires(() => detailToModify != null,
            "No descriptions for that language exist");
        detailToModify!.UpdateDescription(title, goal, methodology);
    }

    #endregion

    #region Private methods

    private static readonly Expression<Func<Training, TrainingStatus>> ValidateStatus =
        training => Enumeration.FromValue<TrainingStatus>(training.StatusId)
            .Validate(training.Identities);

    private List<string> ListMissingFields()
    {
        List<string> errors = new();
        if (!Identities.Any()) errors.Add("Missing Identities");
        if (!Targets.Any()) errors.Add("Missing Targets");
        if (!TrainerEnrollments.Any()) errors.Add("Missing Trainer enrollments");
        if (!Details.Any()) errors.Add("Missing Training Details");
        foreach (var detail in Details)
        {
            errors.AddRange(detail.Validate());
        }

        return errors;
    }

    #endregion
}
