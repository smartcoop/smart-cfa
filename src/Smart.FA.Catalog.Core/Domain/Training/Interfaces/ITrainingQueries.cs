using Core.Domain.Dto;

namespace Core.Domain.Interfaces;

public interface ITrainingQueries
{
    Task<TrainingDto> FindAsync(int trainingId, string language, CancellationToken cancellationToken);
    Task<IEnumerable<TrainingDto>> GetListAsync(int trainerId, string language, CancellationToken cancellationToken);
}
