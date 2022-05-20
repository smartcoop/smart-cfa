using MediatR;
using Microsoft.Extensions.Logging;
using Smart.FA.Catalog.UserAdmin.Application.SeedWork;
using Smart.FA.Catalog.UserAdmin.Domain.LogEvents;
using Smart.FA.Catalog.UserAdmin.Domain.SeedWork;
using Smart.FA.Catalog.UserAdmin.Infrastructure.Persistence;

namespace Smart.FA.Catalog.UserAdmin.Application.UseCases.Commands;

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
        var training = await _catalogContext.Trainings.FindAsync(new object?[] { request.TrainingId }, cancellationToken: cancellationToken);
        _unitOfWork.RegisterDeleted(training!);
        _unitOfWork.Commit();

        _logger.LogInformation(LogEventIds.TrainingDeleted, "Training with id {Id} has been deleted", training!.Id);

        resp.SetSuccess();

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
