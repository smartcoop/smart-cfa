using MediatR;
using Microsoft.Extensions.Logging;
using Smart.FA.Catalog.Application.SeedWork;
using Smart.FA.Catalog.Core.Domain;
using Smart.FA.Catalog.Infrastructure.Persistence;

namespace Smart.FA.Catalog.Application.UseCases.Queries;

public class GetTrainersFromTrainingQueryHandler : IRequestHandler<GetTrainersFromTrainingRequest, GetTrainersFromTrainingResponse>
{
    private readonly ILogger<GetTrainersFromTrainingQueryHandler> _logger;
    private readonly CatalogContext _catalogContext;

    public GetTrainersFromTrainingQueryHandler(ILogger<GetTrainersFromTrainingQueryHandler> logger, CatalogContext catalogContext)
    {
        _logger = logger;
        _catalogContext = catalogContext;
    }
    public async Task<GetTrainersFromTrainingResponse> Handle(GetTrainersFromTrainingRequest request, CancellationToken cancellationToken)
    {
        GetTrainersFromTrainingResponse response = new();
        var training = await _catalogContext.Trainings.FindAsync(new object?[] { request.TrainingId }, cancellationToken: cancellationToken);
        response.Trainers = training?.TrainerAssignments.Select(trainerAssignment => trainerAssignment.Trainer).ToList();
        response.SetSuccess();

        return response;
    }
}

public class GetTrainersFromTrainingRequest: IRequest<GetTrainersFromTrainingResponse>
{
    public int TrainingId { get; set; }
}

public class GetTrainersFromTrainingResponse: ResponseBase
{
    public ICollection<Trainer>? Trainers { get; set; }
}
