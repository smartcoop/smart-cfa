using Application.SeedWork;
using Application.UseCases.Commands;
using Core.Domain;
using Core.Domain.Dto;
using Core.Domain.Enumerations;
using Core.Domain.Interfaces;
using Core.SeedWork;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Queries;

public class
    GetTrainingValidationErrorsQueryHandler : IRequestHandler<GetTrainingValidationErrorsRequest,
        GetTrainingValidationErrorsResponse>
{
    private readonly ILogger<GetTrainingValidationErrorsQueryHandler> _logger;
    private readonly ITrainerRepository _trainerRepository;

    public GetTrainingValidationErrorsQueryHandler(
        ILogger<GetTrainingValidationErrorsQueryHandler> logger
        , ITrainerRepository trainerRepository)
    {
        _logger = logger;
        _trainerRepository = trainerRepository;
    }

    public async Task<GetTrainingValidationErrorsResponse> Handle(GetTrainingValidationErrorsRequest request,
        CancellationToken cancellationToken)
    {
        GetTrainingValidationErrorsResponse resp = new();
        try
        {
            var trainer = await _trainerRepository.FindAsync(request.TrainerId, cancellationToken);
            var training = new Training(trainer, request.Detail, request.Types, request.SlotNumberTypes,
                request.TargetAudiences);
            var errors = training.Validate();

            resp.ValidationErrors = errors;
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

public class GetTrainingValidationErrorsRequest : IRequest<GetTrainingValidationErrorsResponse>
{
    public int TrainerId { get; init; }
    public TrainingDetailDto Detail { get; init; } = null!;
    public List<TrainingTargetAudience> TargetAudiences { get; init; } = null!;
    public List<TrainingType> Types { get; init; } = null!;
    public List<TrainingSlotNumberType> SlotNumberTypes { get; init; } = null!;
}

public class GetTrainingValidationErrorsResponse : ResponseBase
{
    public List<string> ValidationErrors { get; set; } = null!;
}
