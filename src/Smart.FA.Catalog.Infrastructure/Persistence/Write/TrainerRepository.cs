using Core.Domain;
using Core.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Write;

public class TrainerRepository : ITrainerRepository
{
    private readonly Context _context;

    public TrainerRepository(Context context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Trainer>> GetListAsync(int trainingId, CancellationToken cancellationToken)
        => await _context.TrainerEnrollments.Include(enrollment => enrollment.Trainer)
            .Where(enrollment =>enrollment.TrainingId == trainingId)
            .Select(enrollment => enrollment.Trainer)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<Trainer>> GetListAsync(IEnumerable<int> trainingIds, CancellationToken cancellationToken)
        => await _context.TrainerEnrollments.Include(enrollment => enrollment.Trainer)
            .Where(enrollment => trainingIds.Contains(enrollment.TrainingId))
            .Select(enrollment => enrollment.Trainer)
            .ToListAsync(cancellationToken);

    public async Task<Trainer> FindAsync(int trainerId, CancellationToken cancellationToken)
        => await _context.Trainers.FirstOrDefaultAsync(trainer => trainer.Id == trainerId, cancellationToken) ??
           throw new InvalidOperationException();
}
