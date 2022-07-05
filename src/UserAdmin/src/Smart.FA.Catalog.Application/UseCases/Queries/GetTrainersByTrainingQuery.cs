using MediatR;
using Microsoft.Extensions.Logging;
using Smart.FA.Catalog.Application.SeedWork;
using Smart.FA.Catalog.Core.Domain;
using Smart.FA.Catalog.Infrastructure.Persistence;

namespace Smart.FA.Catalog.Application.UseCases.Queries;

public class GetTrainersByTrainingQueryHandler : IRequestHandler<GetTrainersByTrainingRequest, GetTrainersByTrainingResponse>
{
    private readonly ILogger<GetTrainersByTrainingQueryHandler> _logger;
    private readonly CatalogContext _catalogContext;

    public GetTrainersByTrainingQueryHandler(ILogger<GetTrainersByTrainingQueryHandler> logger, CatalogContext catalogContext)
    {
        _logger = logger;
        _catalogContext = catalogContext;
    }
    public async Task<GetTrainersByTrainingResponse> Handle(GetTrainersByTrainingRequest request, CancellationToken cancellationToken)
    {
        GetTrainersByTrainingResponse response = new();
        var training = await _catalogContext.Trainings.FindAsync(new object?[] { request.TrainingId }, cancellationToken: cancellationToken);
        response.Trainers = training?.TrainerAssignments.Select(trainerAssignment => trainerAssignment.Trainer).ToList();
        response.SetSuccess();

        return response;
    }
}

public class GetTrainersByTrainingRequest: IRequest<GetTrainersByTrainingResponse>
{
    public int TrainingId { get; set; }
}

public class GetTrainersByTrainingResponse: ResponseBase
{
    public ICollection<Trainer>? Trainers { get; set; }
}
