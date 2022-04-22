using MediatR;
using Microsoft.Extensions.Logging;
using Smart.FA.Catalog.Application.SeedWork;
using Smart.FA.Catalog.Core.Domain;
using Smart.FA.Catalog.Core.Domain.Dto;
using Smart.FA.Catalog.Core.Domain.Enumerations;
using Smart.FA.Catalog.Core.Domain.Interfaces;
using Smart.FA.Catalog.Core.LogEvents;
using Smart.FA.Catalog.Core.SeedWork;
using Smart.FA.Catalog.Shared.Domain.Enumerations.Training;

namespace Smart.FA.Catalog.Application.UseCases.Commands;

public class CreateTrainingCommandHandler : IRequestHandler<CreateTrainingRequest, CreateTrainingResponse>
{
    private readonly ILogger<CreateTrainingCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITrainerRepository _trainerRepository;

    public CreateTrainingCommandHandler(ILogger<CreateTrainingCommandHandler> logger
        , IUnitOfWork unitOfWork
        , ITrainerRepository trainerRepository)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _trainerRepository = trainerRepository;
    }

    public async Task<CreateTrainingResponse> Handle(CreateTrainingRequest request, CancellationToken cancellationToken)
    {
        CreateTrainingResponse resp = new();

        var trainer = await _trainerRepository.FindAsync(request.TrainerId, cancellationToken);
        var training = new Training(trainer, request.DetailsDto, request.VatExemptionTypes, request.AttendanceTypes, request.TargetAudiences, request.Topics);
        if (!request.IsDraft)
        {
            var result = training.Validate();
            if (result.IsFailure)
            {
                resp.AddErrors(result.Error);
                return resp;
            }
        }

        _unitOfWork.RegisterNew(training);
        _unitOfWork.Commit();
        _logger.LogInformation(LogEventIds.TrainingCreated, "Training with id {Id} has been created", training.Id);

        resp.SetSuccess();

        return resp;
    }
}

public class CreateTrainingRequest : IRequest<CreateTrainingResponse>
{
    public int TrainerId { get; init; }
    public bool IsDraft { get; set; }
    public TrainingLocalizedDetailsDto DetailsDto { get; init; } = null!;
    public List<TrainingTargetAudience> TargetAudiences { get; init; } = null!;
    public List<VatExemptionType> VatExemptionTypes { get; init; } = null!;
    public List<AttendanceType> AttendanceTypes { get; init; } = null!;
    public List<Topic> Topics { get; init; } = null!;
}

public class CreateTrainingResponse : ResponseBase
{
}
