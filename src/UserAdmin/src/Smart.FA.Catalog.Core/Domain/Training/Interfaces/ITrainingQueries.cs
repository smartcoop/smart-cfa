using Smart.FA.Catalog.Core.Domain.Dto;
using Smart.FA.Catalog.Shared.Collections;

namespace Smart.FA.Catalog.Core.Domain.Interfaces;

public interface ITrainingQueries
{
    Task<TrainingDto> FindAsync(int trainingId, string language, CancellationToken cancellationToken);
    Task<IEnumerable<TrainingDto>> GetListAsync(int trainerId, string language, CancellationToken cancellationToken);
    Task<PagedList<TrainingDto>> GetPagedListAsync(int trainerId, string language, PageItem pageItem, CancellationToken cancellationToken);
}
