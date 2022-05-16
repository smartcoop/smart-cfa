using Microsoft.EntityFrameworkCore;
using Smart.FA.Catalog.Shared.Collections;
using Smart.FA.Catalog.Showcase.Infrastructure.Data;
using Smart.FA.Catalog.Showcase.Infrastructure.Data.Extensions;
using Smart.FA.Catalog.Showcase.Web.Mappers;
using Smart.FA.Catalog.Showcase.Web.Pages.Trainer.TrainerDetails;
using Smart.FA.Catalog.Showcase.Web.Pages.Trainer.TrainerList;
using Smart.FA.Catalog.Showcase.Web.Pages.Training.TrainingList;

namespace Smart.FA.Catalog.Showcase.Web.Services.Trainer;

public class TrainerService : ITrainerService
{
    private readonly CatalogShowcaseContext _catalogShowcaseContext;

    public TrainerService(CatalogShowcaseContext catalogShowcaseContext)
    {
        _catalogShowcaseContext = catalogShowcaseContext;
    }

    /// <inheritdoc />
    public async Task<TrainerDetailsViewModel?> GetTrainerDetailsViewModelsByIdAsync(int trainerId, int pageNumber, int pageSize)
    {
        var trainerDetailsWithTopics = await _catalogShowcaseContext.TrainerDetails
            .Where(trainer => trainer.Id == trainerId)
            .ToListAsync();

        if (!trainerDetailsWithTopics.Any())
        {
            return null;
        }

        var trainerPaginatedTrainingLists = await _catalogShowcaseContext.TrainingList
            .Where(training => training.TrainerId == trainerId)
            .GetPaginatedTrainingListsAsync(pageNumber, pageSize);

        var trainingViewModels = trainerPaginatedTrainingLists.ToTrainingListViewModels();

        var trainerDetails = trainerDetailsWithTopics.First();
        return new TrainerDetailsViewModel
        {
            Id = trainerDetails.Id,
            Biography = trainerDetails.Biography,
            FirstName = trainerDetails.FirstName,
            LastName = trainerDetails.LastName,
            Title = trainerDetails.Title,
            Trainings = new PagedList<TrainingListViewModel>(trainingViewModels, new(pageNumber, pageSize), trainerPaginatedTrainingLists.TotalCount),
            SocialNetworks = trainerDetailsWithTopics.ToTrainerSocialNetworks()
        };
    }

    public async Task<PagedList<TrainerListViewModel>> SearchTrainerDetailsViewModelsAsync(string? searchKeyword, int currentPage, int pageSize)
    {
        var trainers = await _catalogShowcaseContext.TrainerList.SearchPaginatedTrainersAsync(searchKeyword, currentPage, pageSize);

        return new PagedList<TrainerListViewModel>(trainers.ToTrainerListViewModels(), new PageItem(currentPage, pageSize), trainers.TotalCount);
    }

    /// <inheritdoc />
    public Task<int> TotalCountAsync()
    {
        return _catalogShowcaseContext.TrainerDetails.TrainerCountAsync();
    }
}
