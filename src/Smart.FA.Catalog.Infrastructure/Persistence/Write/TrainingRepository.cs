using Core.Domain;
using Core.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository;

public class TrainingRepository : ITrainingRepository
{
    private readonly Context _context;

    public TrainingRepository(Context context)
    {
        _context = context;
    }

    public IEnumerable<Training> GetTrainingByTrainerId(int trainerId)
    {
        return _context.TrainerEnrollments
            .Where(enrollment => enrollment.TrainerId == trainerId)
            .Include(enrollment => enrollment.Training)
            .Select(enrollment => enrollment.Training).ToList();
    }

    public async Task<Training> GetTrainingAsync(int trainingId)
    {
        return await _context.Trainings.FindAsync(trainingId);
    }

}
