using Application.SeedWork;
using Core.Domain;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Queries;

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
        try
        {
            var training = await _catalogContext.Trainings.FindAsync(new object?[] { request.TrainingId }, cancellationToken: cancellationToken);
            response.Trainers = training?.TrainerAssignments.Select(ttt => ttt.Trainer).ToList();
            response.SetSuccess();
        }
        catch (Exception e)
        {
             _logger.LogError("{Exception}", e.ToString());
            throw;
        }

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
