using Application.SeedWork;
using Core.Domain;
using Core.Domain.Dto;
using Core.Domain.Enumerations;
using Core.Domain.Interfaces;
using Core.LogEvents;
using Core.SeedWork;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Commands;

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
        try
        {
            var trainer = await _trainerRepository.FindAsync(request.TrainerId, cancellationToken);
            var training = new Training(trainer, request.Detail, request.Types, request.SlotNumberTypes, request.TargetAudiences);
            if (request.IsDraft is false)
            {
                var result = training.Validate();
                await result.OnFailure(errors => throw new Exception(string.Join(Environment.NewLine, errors)));
            }
            _unitOfWork.RegisterNew(training);
            _unitOfWork.Commit();

            _logger.LogInformation(LogEventIds.TrainingCreated, "Training with id {Id} has been created", training.Id);

            resp.SetSuccess();
        }
        catch (Exception e)
        {
            _logger.LogError("{Exception}", e.ToString());
            throw;
        }

        return resp;
    }
}

public class CreateTrainingRequest : IRequest<CreateTrainingResponse>
{
    public int TrainerId { get; init; }
    public bool IsDraft { get; set; }
    public TrainingDetailDto Detail { get; init; } = null!;
    public List<TrainingTargetAudience> TargetAudiences { get; init; } = null!;
    public List<TrainingType> Types { get; init; } = null!;
    public List<TrainingSlotNumberType> SlotNumberTypes { get; init; } = null!;
}

public class CreateTrainingResponse : ResponseBase
{
}
