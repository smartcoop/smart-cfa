using Smart.FA.Catalog.Core.Domain.Dto;
using Smart.FA.Catalog.Shared.Collections;

namespace Smart.FA.Catalog.Core.Domain.Interfaces;

public interface ITrainingQueries
{
    Task<TrainingDto> FindAsync(int trainingId, string language, CancellationToken cancellationToken);
    Task<IEnumerable<TrainingDto>> GetListAsync(int trainerId, string language, CancellationToken cancellationToken);
}
