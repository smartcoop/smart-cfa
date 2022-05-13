using Smart.FA.Catalog.Shared.Collections;
using Smart.FA.Catalog.Showcase.Web.Pages.Trainer.TrainerDetails;
using Smart.FA.Catalog.Showcase.Web.Pages.Trainer.TrainerList;

namespace Smart.FA.Catalog.Showcase.Web.Services.Trainer;

public interface ITrainerService
{
    /// <summary>
    /// Gets the amount of validated trainings.
    /// </summary>
    /// <returns>The number of validated trainings.</returns>
    public Task<int> TotalCountAsync();

    /// <summary>
    /// Retrieves the <see cref="TrainerDetailsViewModel" /> of a given trainer.
    /// </summary>
    /// <param name="trainerId">The id of the trainer to look for.</param>
    /// <param name="pageNumber">Index of the current page.</param>
    /// <param name="pageSize">The maximum number of the trainer's trainings to be fetched.</param>
    /// <returns></returns>
    public Task<TrainerDetailsViewModel?> GetTrainerDetailsViewModelsByIdAsync(int trainerId, int pageNumber, int pageSize);

    /// <summary>
    /// Searches trainers by a keyword.
    /// </summary>
    /// <param name="searchKeyword">The keyword that will be used for the search.</param>
    /// <param name="currentPage">The current page for the pagination.</param>
    /// <param name="pageSize">The number of item returned by the search.</param>
    /// <returns>A task that represents the asynchronous operation. The task's result is a paginated list of <see cref="TrainerDetailsViewModel"/>.</returns>
    public Task<PagedList<TrainerListViewModel>> SearchTrainerDetailsViewModelsAsync(string? searchKeyword, int currentPage, int pageSize);
}
