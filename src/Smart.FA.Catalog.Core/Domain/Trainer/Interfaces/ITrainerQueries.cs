using Core.Domain.Dto;

namespace Core.Domain.Interfaces;

public interface ITrainerQueries
{
    Task<IEnumerable<TrainerDto>> GetListAsync(List<int> trainingIds, CancellationToken cancellationToken);
}
