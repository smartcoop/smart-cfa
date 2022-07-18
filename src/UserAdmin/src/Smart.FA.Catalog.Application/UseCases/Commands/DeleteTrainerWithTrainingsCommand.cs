using MediatR;
using Microsoft.EntityFrameworkCore;
using Smart.FA.Catalog.Application.SeedWork;
using Smart.FA.Catalog.Infrastructure.Persistence;

namespace Smart.FA.Catalog.Application.UseCases.Commands;

public class DeleteTrainerWithTrainingsCommand : IRequestHandler<DeleteTrainerWithTrainingsRequest, DeleteTrainerWithTrainingsResponse>
{
    private readonly CatalogContext _catalogContext;

    public DeleteTrainerWithTrainingsCommand(CatalogContext catalogContext)
    {
        _catalogContext = catalogContext;
    }

    public async Task<DeleteTrainerWithTrainingsResponse> Handle(DeleteTrainerWithTrainingsRequest command, CancellationToken cancellationToken)
    {
        DeleteTrainerWithTrainingsResponse withTrainingsResponse = new();
        var trainer = await _catalogContext.Trainers.FirstOrDefaultAsync(trainer => trainer.Id == command.TrainerId, cancellationToken);
        if (trainer is not null)
        {
            // Select all trainings created by the trainer for deletion
            var trainingListOfTrainer = trainer.Assignments
                .Select(assignment => assignment.Training)
                .Where(training => training.CreatedBy == trainer.Id);

            _catalogContext.Remove(trainer);
            _catalogContext.RemoveRange(trainingListOfTrainer);

            await _catalogContext.SaveChangesAsync(cancellationToken);
        }

        withTrainingsResponse.SetSuccess();
        return withTrainingsResponse;
    }
}

public class DeleteTrainerWithTrainingsRequest : IRequest<DeleteTrainerWithTrainingsResponse>
{
    public int TrainerId { get; set; }
}

public class DeleteTrainerWithTrainingsResponse : ResponseBase
{
}
