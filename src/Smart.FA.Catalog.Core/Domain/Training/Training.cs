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
    public TrainingStatus Status { get; private set; } = TrainingStatus.Draft;

    #endregion

    #region Constructors

    public Training(Trainer trainer, TrainingDetailDto trainingDetail, IEnumerable<TrainingType> types,
        IEnumerable<TrainingSlotNumberType> slotNumberTypes,
        IEnumerable<TrainingTargetAudience> targetAudiences)
    {
        AddDetails(trainingDetail.Title!, trainingDetail.Goal!, trainingDetail.Methodology!,
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
        Guard.Requires(() => trainer is not null, "There should be at least one trainer enrolled (owner)");
        TrainerEnrollment trainerEnrollment = new(this, trainer!);
        if (_trainerEnrollments.Contains(trainerEnrollment)) throw new Exception();
        _trainerEnrollments.Add(trainerEnrollment);
    }

    public void EnrollTrainers(IEnumerable<Trainer> trainers)
    {
        DisEnrollAll();
        foreach (var trainer in trainers)
        {
            EnrollTrainer(trainer);
        }
    }

    public void DisEnrollAll()
        => _trainerEnrollments.RemoveAll(enrollment => enrollment.TrainerId != TrainerCreatorId);

    public Result<Training, IEnumerable<Error>> Validate()
    {
        var validator = new TrainingValidator();
        var validationResult =  validator.Validate(this);

        if (validationResult.IsValid is false)
        {
          var errors = validationResult.Errors.Select( validationError => Errors.Training.ValidationError(validationError.ErrorMessage));
          return Result.Failure<Training, IEnumerable<Error>>(errors);
        }

        Status = Status.Validate(_identities);

        var trainingDetail = Details.FirstOrDefault(training => training.Language == Language.Create("EN").Value) ?? Details.First();
        AddDomainEvent(new ValidateTrainingEvent(trainingDetail.Title!, Id, TrainerEnrollments.Select(enrollment => enrollment.TrainerId)));

        return Result.Success<Training, IEnumerable<Error>>(this);
    }

    public void AddDetails(string title, string goal, string methodology, Language language)
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
