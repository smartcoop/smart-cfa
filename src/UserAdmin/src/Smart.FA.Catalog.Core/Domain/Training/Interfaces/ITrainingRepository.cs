using Smart.FA.Catalog.Core.SeedWork;

namespace Smart.FA.Catalog.Core.Domain.Interfaces;

public interface ITrainingRepository
{
    Task<IReadOnlyList<Training>> GetListAsync(int trainerId, Specification<Training> specification, CancellationToken cancellationToken = default);
    Task<Training?> GetFullAsync(int trainingId, CancellationToken cancellationToken = default);
    Task<Training> FindAsync(int trainingId, CancellationToken cancellationToken = default);
}
