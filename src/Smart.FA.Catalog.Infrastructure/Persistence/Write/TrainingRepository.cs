using Core.Domain;
using Core.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Write;

public class TrainingRepository : ITrainingRepository
{
    private readonly Context _context;

    public TrainingRepository(Context context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Training>> GetTrainingsByTrainerIdAsync(int trainerId, CancellationToken cancellationToken)
        => await _context.Trainings.Include(training => training.TrainerEnrollments).Where(training =>
            training.TrainerEnrollments.Select(enrollment => enrollment.TrainerId).Contains(trainerId)).ToListAsync(cancellationToken);

    public async Task<Training?> GetFullTraGetTrainingAsync(int trainingId, CancellationToken cancellationToken)
        => await _context.Trainings.Include(training => training.TrainerEnrollments)
            .Include(training => training.Details)
            .Include(training => training.Identities)
            .Include(training => training.Slots)
            .Include(training => training.Targets)
            .FirstOrDefaultAsync(training => training.Id == trainingId, cancellationToken);

    public async Task<Training> FindTrainingIdAsync(int trainingId, CancellationToken cancellationToken)
        => await _context.Trainings.FindAsync(new object?[] { trainingId, cancellationToken }, cancellationToken: cancellationToken) ?? throw new Exception();
}
