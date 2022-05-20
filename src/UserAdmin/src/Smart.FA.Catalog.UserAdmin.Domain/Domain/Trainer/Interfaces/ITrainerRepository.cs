namespace Smart.FA.Catalog.UserAdmin.Domain.Domain.Interfaces;

public interface ITrainerRepository
{
    Task<IReadOnlyList<Trainer>> GetListAsync(int trainingId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Trainer>> GetListAsync(IEnumerable<int> trainingIds, CancellationToken cancellationToken = default);
    Task<Trainer> FindAsync(int trainerId, CancellationToken cancellationToken = default);
}
