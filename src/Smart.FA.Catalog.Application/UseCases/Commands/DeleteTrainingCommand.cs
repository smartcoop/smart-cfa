using Application.SeedWork;
using Core.LogEvents;
using Core.SeedWork;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Commands;

public class DeleteTrainingCommand: IRequestHandler<DeleteTrainingRequest, DeleteTrainingResponse>
{
    private readonly ILogger<DeleteTrainingCommand> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly Context _context;

    public DeleteTrainingCommand(ILogger<DeleteTrainingCommand> logger, IUnitOfWork unitOfWork, Context context)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _context = context;
    }
    public async Task<DeleteTrainingResponse> Handle(DeleteTrainingRequest request, CancellationToken cancellationToken)
    {
        DeleteTrainingResponse resp = new();

        try
        {
            var training = await _context.Trainings.FindAsync(new object?[] { request.TrainingId }, cancellationToken: cancellationToken);
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
