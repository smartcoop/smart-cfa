using Core.Domain;
using Core.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository;

public class TrainerRepository: ITrainerRepository
{
    private readonly Context _context;

    public TrainerRepository(Context context)
    {
        _context = context;
    }
    public IEnumerable<Trainer> GetTrainersByTrainingId(int trainingId)
    {
        return  _context.TrainerEnrollments
            .Where(enrollment => enrollment.TrainerId == trainingId)
            .Include(enrollment => enrollment.Trainer)
            .Select(enrollment => enrollment.Trainer);
    }

    public async Task<Trainer> FindByIdAsync(int trainerId) => await _context.Trainers.FirstOrDefaultAsync(trainer => trainer.Id == trainerId) ?? throw new InvalidOperationException();
}
