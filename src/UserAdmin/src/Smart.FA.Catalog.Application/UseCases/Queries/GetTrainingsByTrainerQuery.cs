using MediatR;
using Microsoft.Extensions.Logging;
using Smart.FA.Catalog.Application.SeedWork;
using Smart.FA.Catalog.Core.Domain.Dto;
using Smart.FA.Catalog.Core.Domain.Interfaces;
using Smart.FA.Catalog.Core.Domain.ValueObjects;

namespace Smart.FA.Catalog.Application.UseCases.Queries;

public class GetTrainingsByTrainerQueryHandler : IRequestHandler<GetTrainingsByTrainerRequest, GetTrainingsByTrainerResponse>
{
    private readonly ILogger<GetTrainingsByTrainerQueryHandler> _logger;
    private readonly ITrainingQueries _trainingQueries;

    public GetTrainingsByTrainerQueryHandler(ILogger<GetTrainingsByTrainerQueryHandler> logger,
        ITrainingQueries trainingQueries )
    {
        _logger = logger;
        _trainingQueries = trainingQueries;
    }

    public async Task<GetTrainingsByTrainerResponse> Handle(GetTrainingsByTrainerRequest request, CancellationToken cancellationToken)
    {
        GetTrainingsByTrainerResponse resp = new();
        resp.Trainings = await _trainingQueries.GetListAsync(request.TrainerId, request.Language.Value, cancellationToken);
        resp.SetSuccess();
        return resp;
    }
}

public class GetTrainingsByTrainerRequest : IRequest<GetTrainingsByTrainerResponse>
{
    public int TrainerId { get; init; }
    public Language Language { get; init; } = null!;
}

public class GetTrainingsByTrainerResponse : ResponseBase
{
    public IEnumerable<TrainingDto>? Trainings { get; set; }
}
