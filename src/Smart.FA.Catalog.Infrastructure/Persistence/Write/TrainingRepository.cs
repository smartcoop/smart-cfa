using Microsoft.EntityFrameworkCore;
using Smart.FA.Catalog.Core.Domain;
using Smart.FA.Catalog.Core.Domain.Interfaces;
using Smart.FA.Catalog.Core.SeedWork;

namespace Smart.FA.Catalog.Infrastructure.Persistence.Write;

public class TrainingRepository : ITrainingRepository
{
    private readonly CatalogContext _catalogContext;

    public TrainingRepository(CatalogContext catalogContext)
    {
        _catalogContext = catalogContext;
    }

    public async Task<IReadOnlyList<Training>> GetListAsync(int trainerId, Specification<Training> specification, CancellationToken cancellationToken)
        => await _catalogContext
            .Trainings
            .Include(training => training.TrainerAssignments)
            .Where(specification.ToExpression())
            .Where(training =>
                        training
                            .TrainerAssignments
                            .Select(assignment => assignment.TrainerId)
                            .Contains(trainerId))
            .ToListAsync(cancellationToken);

    public async Task<Training?> GetFullAsync(int trainingId, CancellationToken cancellationToken)
        => await _catalogContext.Trainings.Include(training => training.TrainerAssignments)
            .Include(training => training.Details)
            .Include(training => training.Identities)
            .Include(training => training.Slots)
            .Include(training => training.Targets)
            .Include(training => training.Topics)
            .FirstOrDefaultAsync(training => training.Id == trainingId, cancellationToken);

    public async Task<Training> FindAsync(int trainingId, CancellationToken cancellationToken)
        => await _catalogContext.Trainings.SingleAsync( training => training.Id == trainingId, cancellationToken);
}
