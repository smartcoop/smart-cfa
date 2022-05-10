using MediatR;
using Microsoft.Extensions.Logging;
using Smart.FA.Catalog.Application.SeedWork;
using Smart.FA.Catalog.Core.Domain.Dto;
using Smart.FA.Catalog.Core.Domain.Interfaces;
using Smart.FA.Catalog.Core.Domain.ValueObjects;
using Smart.FA.Catalog.Shared.Collections;

namespace Smart.FA.Catalog.Application.UseCases.Queries;

public class GetPagedTrainingsFromTrainerQueryHandler : IRequestHandler<GetPagedTrainingsFromTrainerRequest,
    GetPagedTrainingsFromTrainerResponse>
{
    private readonly ILogger<GetPagedTrainingsFromTrainerQueryHandler> _logger;
    private readonly ITrainingQueries _trainingQueries;

    public GetPagedTrainingsFromTrainerQueryHandler(ILogger<GetPagedTrainingsFromTrainerQueryHandler> logger,
        ITrainingQueries trainingQueries)
    {
        _logger = logger;
        _trainingQueries = trainingQueries;
    }


    public async Task<GetPagedTrainingsFromTrainerResponse> Handle(GetPagedTrainingsFromTrainerRequest request,
        CancellationToken cancellationToken)
    {
        GetPagedTrainingsFromTrainerResponse resp = new();

        try
        {
            resp.Trainings =
                 await _trainingQueries.GetPagedListAsync(request.TrainerId, request.Language.Value, request.PageItem, cancellationToken);

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

public class GetPagedTrainingsFromTrainerRequest : IRequest<GetPagedTrainingsFromTrainerResponse>
{
    public int TrainerId { get; init; }
    public Language Language { get; init; } = null!;
    public PageItem PageItem { get; init; } = null!;
}

public class GetPagedTrainingsFromTrainerResponse : ResponseBase
{
    public PagedList<TrainingDto>? Trainings { get; set; }
}
