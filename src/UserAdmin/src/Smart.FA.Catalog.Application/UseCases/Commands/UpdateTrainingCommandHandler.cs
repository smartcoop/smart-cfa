using MediatR;
using Microsoft.Extensions.Logging;
using Smart.FA.Catalog.Application.SeedWork;
using Smart.FA.Catalog.Core.Domain;
using Smart.FA.Catalog.Core.Domain.Dto;
using Smart.FA.Catalog.Core.Domain.Interfaces;
using Smart.FA.Catalog.Core.Domain.ValueObjects;
using Smart.FA.Catalog.Core.Exceptions;
using Smart.FA.Catalog.Core.LogEvents;
using Smart.FA.Catalog.Core.SeedWork;
using Smart.FA.Catalog.Core.Services;
using Smart.FA.Catalog.Shared.Domain.Enumerations.Training;

namespace Smart.FA.Catalog.Application.UseCases.Commands;

public class UpdateTrainingCommandHandler : IRequestHandler<UpdateTrainingRequest, UpdateTrainingResponse>
{
    private readonly ILogger<UpdateTrainingCommandHandler> _logger;
    private readonly ITrainingRepository _trainingRepository;
    private readonly ITrainerRepository _trainerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMailService _mailService;

    public UpdateTrainingCommandHandler
    (
        ILogger<UpdateTrainingCommandHandler> logger
        , ITrainingRepository trainingRepository
        , ITrainerRepository trainerRepository
        , IUnitOfWork unitOfWork
        , IMailService mailService
    )
    {
        _logger = logger;
        _trainingRepository = trainingRepository;
        _trainerRepository = trainerRepository;
        _unitOfWork = unitOfWork;
        _mailService = mailService;
    }

    public async Task<UpdateTrainingResponse> Handle(UpdateTrainingRequest request, CancellationToken cancellationToken)
    {
        UpdateTrainingResponse resp = new();
        var training = await _trainingRepository.GetFullAsync(request.TrainingId, cancellationToken);

        if (training is null) throw new TrainingException(Errors.Training.NotFound(request.TrainingId));

        training.UpdateDetails(request.DetailsDto.Title!, request.DetailsDto.Goal!, request.DetailsDto.Methodology!, request.DetailsDto.PracticalModalities,
            Language.Create(request.DetailsDto.Language).Value);
        training.MarkAsGivenBySmart(request.IsGivenBySmart);
        training.SwitchVatExemptionTypes(request.VatExemptionTypes);
        training.SwitchTargetAudience(request.TargetAudienceTypes);
        training.SwitchAttendanceTypes(request.AttendanceTypes);
        training.SwitchTopics(request.Topics);
        var trainers = await _trainerRepository.GetListAsync(request.TrainingId, cancellationToken);
        training.AssignTrainers(trainers);

        var newStatus = request.IsDraft ? TrainingStatusType.Draft : TrainingStatusType.Validated;

        var result = training.ChangeStatus(newStatus);

        if (result.IsFailure)
        {
            resp.AddErrors(result.Error);
            return resp;
        }

        _unitOfWork.RegisterDirty(training);
        _unitOfWork.Commit();

        _logger.LogInformation(LogEventIds.TrainingUpdated, "Training with id {Id} has been updated", training.Id);

        resp.Training = training;
        resp.SetSuccess();

        return resp;
    }
}

public class UpdateTrainingRequest : IRequest<UpdateTrainingResponse>
{
    public int TrainingId { get; init; }
    public TrainingLocalizedDetailsDto DetailsDto { get; init; } = null!;
    public List<TargetAudienceType>? TargetAudienceTypes { get; init; }
    public List<VatExemptionType> VatExemptionTypes { get; init; } = null!;
    public List<AttendanceType> AttendanceTypes { get; init; } = null!;
    public List<Topic> Topics { get; init; } = null!;
    public List<int> TrainerIds { get; init; } = null!;
    public bool IsDraft { get; set; }
    public bool IsGivenBySmart { get; set; }
}

public class UpdateTrainingResponse : ResponseBase
{
    public Training Training { get; set; } = null!;
}
