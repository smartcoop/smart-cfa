using Smart.FA.Catalog.Core.Domain.Dto;

namespace Smart.FA.Catalog.Core.Domain.Interfaces;

public interface ITrainerQueries
{
    Task<IEnumerable<TrainerDto>> GetListAsync(List<int> trainingIds, CancellationToken cancellationToken);
}
