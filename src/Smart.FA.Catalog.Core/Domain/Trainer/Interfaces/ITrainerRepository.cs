namespace Core.Domain.Interfaces;

public interface ITrainerRepository
{
    Task<IEnumerable<Trainer>> GetListAsync(int trainingId, CancellationToken cancellationToken);
    Task<IEnumerable<Trainer>> GetListAsync(IEnumerable<int> trainingIds, CancellationToken cancellationToken);
    Task<Trainer> FindAsync(int trainerId, CancellationToken cancellationToken);
}
