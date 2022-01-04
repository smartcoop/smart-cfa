using Application.SeedWork;
using Core.Domain;
using Core.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Queries;

public class
    GetTrainingsFromTrainerQueryHandler : IRequestHandler<GetTrainingsFromTrainerRequest,
        GetTrainingsFromTrainerResponse>
{
    private readonly ILogger<GetTrainingsFromTrainerQueryHandler> _logger;
    private readonly ITrainingRepository _trainingRepository;

    public GetTrainingsFromTrainerQueryHandler(ILogger<GetTrainingsFromTrainerQueryHandler> logger,
        ITrainingRepository trainingRepository)
    {
        _logger = logger;
        _trainingRepository = trainingRepository;
    }

    public Task<GetTrainingsFromTrainerResponse> Handle(GetTrainingsFromTrainerRequest request,
        CancellationToken cancellationToken)
    {
        GetTrainingsFromTrainerResponse resp = new();

        try
        {
            resp.Trainings = _trainingRepository.GetTrainingByTrainerId(request.TrainerId);
            resp.SetSuccess();
        }
        catch (Exception e)
        {
            _logger.LogError(e.StackTrace);
            throw;
        }

        return Task.FromResult(resp);
    }
}

public class GetTrainingsFromTrainerRequest : IRequest<GetTrainingsFromTrainerResponse>
{
    public int TrainerId { get; set; }
}

public class GetTrainingsFromTrainerResponse : ResponseBase
{
    public IEnumerable<Training> Trainings { get; set; }
}
