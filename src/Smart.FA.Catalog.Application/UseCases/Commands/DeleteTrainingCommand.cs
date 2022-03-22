using MediatR;
using Microsoft.Extensions.Logging;
using Smart.FA.Catalog.Application.SeedWork;
using Smart.FA.Catalog.Core.LogEvents;
using Smart.FA.Catalog.Core.SeedWork;
using Smart.FA.Catalog.Infrastructure.Persistence;

namespace Smart.FA.Catalog.Application.UseCases.Commands;

public class DeleteTrainingCommand: IRequestHandler<DeleteTrainingRequest, DeleteTrainingResponse>
{
    private readonly ILogger<DeleteTrainingCommand> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly CatalogContext _catalogContext;

    public DeleteTrainingCommand(ILogger<DeleteTrainingCommand> logger, IUnitOfWork unitOfWork, CatalogContext catalogContext)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _catalogContext = catalogContext;
    }
    public async Task<DeleteTrainingResponse> Handle(DeleteTrainingRequest request, CancellationToken cancellationToken)
    {
        DeleteTrainingResponse resp = new();

        try
        {
            var training = await _catalogContext.Trainings.FindAsync(new object?[] { request.TrainingId }, cancellationToken: cancellationToken);
            _unitOfWork.RegisterDeleted(training!);
            _unitOfWork.Commit();

            _logger.LogInformation(LogEventIds.TrainingDeleted, "Training with id {Id} has been deleted", training!.Id);

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

public class DeleteTrainingRequest: IRequest<DeleteTrainingResponse>
{
    public int TrainingId { get; set; }
}

public class DeleteTrainingResponse : ResponseBase
{

}
