using Smart.FA.Catalog.Showcase.Web.Pages.Trainer.TrainerDetails;

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
}
