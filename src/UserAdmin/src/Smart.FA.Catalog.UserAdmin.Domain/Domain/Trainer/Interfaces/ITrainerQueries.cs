using Smart.FA.Catalog.UserAdmin.Domain.Domain.Dto;

namespace Smart.FA.Catalog.UserAdmin.Domain.Domain.Interfaces;

public interface ITrainerQueries
{
    Task<IEnumerable<TrainerDto>> GetListAsync(List<int> trainingIds, CancellationToken cancellationToken);
}
