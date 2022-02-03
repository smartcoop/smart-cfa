using Application.SeedWork;
using Core.Domain;
using Core.Domain.Dto;
using Core.Domain.Interfaces;
using Core.SeedWork;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Queries;

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
