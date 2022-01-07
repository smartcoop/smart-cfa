namespace Core.Domain.Interfaces;

public interface ITrainingRepository
{
    Task<IEnumerable<Training>> GetTrainingsByTrainerIdAsync(int trainerId, CancellationToken cancellationToken);
    Task<Training?> GetFullTraGetTrainingAsync(int trainingId, CancellationToken cancellationToken);

}
