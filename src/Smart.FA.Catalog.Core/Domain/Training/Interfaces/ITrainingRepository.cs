namespace Core.Domain.Interfaces;

public interface ITrainingRepository
{
    IEnumerable<Training> GetTrainingByTrainerId(int trainerId);
}
