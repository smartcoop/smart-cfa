using Smart.FA.Catalog.Shared.Collections;
using Smart.FA.Catalog.Showcase.Web.Pages.Training.TrainingList;

namespace Smart.FA.Catalog.Showcase.Web.Services.Training;

public interface ITrainingService
{
    public Task<PagedList<TrainingListViewModel>> GetPaginatedTrainingViewModelsAsync(int pageNumber, int pageSize, bool random = false);

    public Task<PagedList<TrainingListViewModel>> GetPaginatedTrainingViewModelsByTrainerIdAsync(int trainerId, int pageNumber, int pageSize);

    /// <summary>
    /// Searches trainings by a keyword.
    /// </summary>
    /// <param name="searchKeyword">The keyword that will be used for the search.</param>
    /// <param name="currentPage">The current page for the pagination.</param>
    /// <param name="pageSize">The number of item returned by the search.</param>
    /// <returns>A task that represents the asynchronous operation. The task's result is a paginated list of <see cref="TrainingListViewModel"/>.</returns>
    public Task<PagedList<TrainingListViewModel>> SearchTrainingViewModelsAsync(string? searchKeyword, int currentPage, int pageSize);
}
