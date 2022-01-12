namespace Core.Domain.Interfaces;

public interface ITrainerRepository
{
    Task<IReadOnlyList<Trainer>> GetListAsync(int trainingId, CancellationToken cancellationToken);
    Task<IReadOnlyList<Trainer>> GetListAsync(IEnumerable<int> trainingIds, CancellationToken cancellationToken);
    Task<Trainer> FindAsync(int trainerId, CancellationToken cancellationToken);
}
