using Core.Domain.Dto;
using Core.Domain.Enumerations;
using Core.Domain.Validators;
using Core.Exceptions;
using Core.SeedWork;
using CSharpFunctionalExtensions;
using Entity = Core.SeedWork.Entity;

namespace Core.Domain;

public class Training : Entity, IAggregateRoot
{
    #region Private fields

    private readonly List<TrainerAssignment> _trainerAssignments = new();
    private readonly List<TrainingIdentity> _identities = new();
    private readonly List<TrainingTarget> _targets = new();
    private readonly List<TrainingDetail> _details = new();
    private readonly List<TrainingSlot> _slots = new();
    private readonly List<TrainingCategory> _topics = new();

    #endregion

    #region Properties

    public virtual IReadOnlyCollection<TrainerAssignment> TrainerAssignments => _trainerAssignments.AsReadOnly();
    public virtual IReadOnlyCollection<TrainingIdentity> Identities => _identities.AsReadOnly();
    public virtual IReadOnlyCollection<TrainingTarget> Targets => _targets.AsReadOnly();
    public virtual IReadOnlyCollection<TrainingDetail> Details => _details.AsReadOnly();
    public virtual IReadOnlyCollection<TrainingSlot> Slots => _slots.AsReadOnly();
    public virtual IReadOnlyCollection<TrainingCategory> Topics => _topics.AsReadOnly();

    public int TrainerCreatorId { get; }
    public TrainingStatus Status { get; private set; } = TrainingStatus.Draft;

    #endregion

    #region Constructors

    public Training
    (
        Trainer trainer
        , TrainingDetailDto detail
        , IEnumerable<TrainingType> types
        , IEnumerable<TrainingSlotNumberType> slotNumberTypes
        , IEnumerable<TrainingTargetAudience> targetAudiences
        , IEnumerable<TrainingTopic> topics
    )
    {
        AddDetails(detail.Title!, detail.Goal, detail.Methodology,
            Language.Create(detail.Language).Value);
        SwitchTrainingTypes(types);
        SwitchTargetAudience(targetAudiences);
        SwitchSlotNumberType(slotNumberTypes);
        SwitchTopics(topics);
        AssignTrainer(trainer);
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

    public void SwitchTopics(IEnumerable<TrainingTopic>? topics)
    {
        Guard.Requires(() => topics != null, "topics should not be null");
        _topics.Clear();
        _topics.AddRange(topics!.Distinct()
            .Select(topic => new TrainingCategory(this, topic)));
    }

    public void AssignTrainer(Trainer? trainer)
    {
        Guard.Requires(() => trainer is not null, "There should be at least one trainer assigned (owner)");
        TrainerAssignment trainerAssignment = new(this, trainer!);
        if (_trainerAssignments.Contains(trainerAssignment)) throw new Exception();
        _trainerAssignments.Add(trainerAssignment);
    }

    public void AssignTrainers(IEnumerable<Trainer> trainers)
    {
        UnAssignAll();
        foreach (var trainer in trainers)
        {
            AssignTrainer(trainer);
        }
    }

    public void UnAssignAll()
        => _trainerAssignments.RemoveAll(assignment => assignment.TrainerId != TrainerCreatorId);

    public Result<Training, IEnumerable<Error>> Validate()
    {
        var validator = new TrainingValidator();
        var validationResult = validator.Validate(this);

        if (validationResult.IsValid is false)
        {
            var errors = validationResult.Errors.Select(validationError =>
                Errors.Training.ValidationError(validationError.ErrorMessage));
            return Result.Failure<Training, IEnumerable<Error>>(errors);
        }

        Status = Status.Validate(_identities);

        var trainingDetail = Details.FirstOrDefault(training => training.Language == Language.Create("EN").Value) ??
                             Details.First();
        AddDomainEvent(new ValidateTrainingEvent(trainingDetail.Title!, Id,
            TrainerAssignments.Select(assignment => assignment.TrainerId)));

        return Result.Success<Training, IEnumerable<Error>>(this);
    }

    public void AddDetails(string title, string? goal, string? methodology, Language language)
    {
        Guard.AgainstNull(title, nameof(title));
        Guard.Requires(() => _details.FirstOrDefault(detail => detail.Language.Value == language.Value) == null,
            "A description for that language already exists");
        _details.Add(new TrainingDetail(this, title!, goal, methodology, language));
    }

    public void UpdateDetails(string title, string goal, string methodology, Language language)
    {
        var detailToModify = _details.FirstOrDefault(detail => detail.Language.Value == language.Value);
        Guard.Requires(() => detailToModify != null,
            "No descriptions for that language exist");
        detailToModify!.UpdateDescription(title, goal, methodology);
    }

    #endregion
}
