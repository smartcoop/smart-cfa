using Smart.FA.Catalog.Shared.Collections;
using Smart.FA.Catalog.Showcase.Web.Pages.Training.TrainingList;

namespace Smart.FA.Catalog.Showcase.Web.Services.Training;

public interface ITrainingService
{
    public Task<PagedList<TrainingListViewModel>> GetPaginatedTrainingViewModelsAsync(int pageNumber, int pageSize, bool random = false);

    public Task<PagedList<TrainingListViewModel>> GetPaginatedTrainingViewModelsByTrainerIdAsync(int trainerId, int pageNumber, int pageSize);
}
