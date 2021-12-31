namespace Core.Domain.Interfaces;

public interface ITrainerRepository
{
    IEnumerable<Trainer> GetTrainersByTrainingId(int trainingId);
    Task<Trainer> FindByIdAsync(int trainerId);
}
