using CSharpFunctionalExtensions;
using Smart.FA.Catalog.Core.Domain.Dto;
using Smart.FA.Catalog.Core.Domain.Enumerations;
using Smart.FA.Catalog.Core.Domain.Validators;
using Smart.FA.Catalog.Core.Domain.ValueObjects;
using Smart.FA.Catalog.Core.Exceptions;
using Smart.FA.Catalog.Core.SeedWork;
using Smart.FA.Catalog.Shared.Collections;
using Smart.FA.Catalog.Shared.Domain.Enumerations.Training;

namespace Smart.FA.Catalog.Core.Domain;

public class Training : SeedWork.Entity, IAggregateRoot
{
    #region Private fields

    private readonly List<TrainerAssignment> _trainerAssignments = new();
    private readonly List<VatExemptionClaim> _vatExemptionClaims = new();
    private readonly List<TrainingTargetAudience> _targets = new();
    private readonly List<TrainingLocalizedDetails> _details = new();
    private readonly List<TrainingAttendance> _attendances = new();
    private readonly List<TrainingTopic> _topics = new();

    #endregion

    #region Properties

    public virtual IReadOnlyCollection<TrainerAssignment> TrainerAssignments => _trainerAssignments.AsReadOnly();
    public virtual IReadOnlyCollection<VatExemptionClaim> VatExemptionClaims => _vatExemptionClaims.AsReadOnly();
    public virtual IReadOnlyCollection<TrainingTargetAudience> Targets => _targets.AsReadOnly();
    public virtual IReadOnlyCollection<TrainingLocalizedDetails> Details => _details.AsReadOnly();
    public virtual IReadOnlyCollection<TrainingAttendance> Attendances => _attendances.AsReadOnly();
    public virtual IReadOnlyCollection<TrainingTopic> Topics => _topics.AsReadOnly();
    public int TrainerCreatorId { get; }
    public TrainingStatusType StatusType { get; private set; } = TrainingStatusType.Draft;
    public bool IsGivenBySmart { get; private set; }

    #endregion

    #region Constructors

    public Training
    (
        Trainer trainer
        , Dto.TrainingLocalizedDetailsDto detailDto
        , IEnumerable<VatExemptionType> vatExemptionTypes
        , IEnumerable<AttendanceType> attendanceTypes
        , IEnumerable<TargetAudienceType> targetAudiences
        , IEnumerable<Topic> topics
    )
    {
        AddDetails(detailDto.Title!, detailDto.Goal, detailDto.Methodology, detailDto.PracticalModalities, Language.Create(detailDto.Language).Value);
        SwitchVatExemptionTypes(vatExemptionTypes);
        SwitchTargetAudience(targetAudiences);
        SwitchAttendanceTypes(attendanceTypes);
        SwitchTopics(topics);
        AssignTrainer(trainer);
        TrainerCreatorId = trainer.Id;
    }

    protected Training()
    {
    }

    #endregion

    #region Public methods

    public void MarkAsGivenBySmart(bool isFromSmart = true )
    {
        IsGivenBySmart = isFromSmart;
    }

    public void SwitchVatExemptionTypes(IEnumerable<VatExemptionType>? vatExemptionTypes)
    {
        Guard.AgainstNull(vatExemptionTypes, nameof(vatExemptionTypes));
        _vatExemptionClaims.Clear();
        _vatExemptionClaims.AddRange(vatExemptionTypes!.Distinct()
            .Select(vatExemptionType => new VatExemptionClaim(this, vatExemptionType)));
    }

    public void SwitchTargetAudience(IEnumerable<TargetAudienceType>? trainingTargetAudiences)
    {
        Guard.Requires(() => trainingTargetAudiences != null, "training audiences should not be null");
        _targets.Clear();
        _targets.AddRange(trainingTargetAudiences!.Distinct()
            .Select(trainingAudience => new TrainingTargetAudience(this, trainingAudience)));
    }

    public void SwitchAttendanceTypes(IEnumerable<AttendanceType>? attendanceTypes)
    {
        Guard.AgainstNull(attendanceTypes, nameof(attendanceTypes));
        _attendances.Clear();
        _attendances.AddRange(attendanceTypes!.Distinct()
            .Select(attendanceType => new TrainingAttendance(this, attendanceType)));
    }

    public void SwitchTopics(IEnumerable<Topic>? topics)
    {
        Guard.Requires(() => topics != null, "topics should not be null");
        _topics.Clear();
        _topics.AddRange(topics!.Distinct()
            .Select(topic => new TrainingTopic(this, topic)));
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

        StatusType = StatusType.Validate(_vatExemptionClaims);

        var details = Details.FirstOrDefault(training => training.Language == Language.Create("EN").Value) ??
                             Details.First();
        AddDomainEvent(new ValidateTrainingEvent(details.Title!, Id, TrainerAssignments.Select(assignment => assignment.TrainerId)));

        return Result.Success<Training, IEnumerable<Error>>(this);
    }

    public void AddDetails(string title, string? goal, string? methodology, string? practicalModalities, Language language)
    {
        Guard.AgainstNull(title, nameof(title));
        Guard.Requires(() => _details.FirstOrDefault(details => details.Language.Value == language.Value) == null,
            "A description for that language already exists");
        _details.Add(new TrainingLocalizedDetails(this, title!, goal, methodology, practicalModalities, language));
    }

    public void UpdateDetails(string title, string? goal, string? methodology, string? practicalModalities, Language language)
    {
        var detailsToEdit = _details.FirstOrDefault(details => details.Language.Value == language.Value);
        Guard.AgainstNull(detailsToEdit, nameof(detailsToEdit));
        detailsToEdit!.UpdateDescription(title, goal, methodology, practicalModalities);
    }

    #endregion
}
