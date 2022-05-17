using Smart.FA.Catalog.Shared.Collections;
using Smart.FA.Catalog.Showcase.Infrastructure.Data;
using Smart.FA.Catalog.Showcase.Infrastructure.Data.Extensions;
using Smart.FA.Catalog.Showcase.Web.Mappers;
using Smart.FA.Catalog.Showcase.Web.Pages.Training.TrainingList;

namespace Smart.FA.Catalog.Showcase.Web.Services.Training;

public class TrainingService : ITrainingService
{
    private readonly CatalogShowcaseContext _catalogShowcaseContext;

    public TrainingService(CatalogShowcaseContext catalogShowcaseContext)
    {
        _catalogShowcaseContext = catalogShowcaseContext;
    }

    public async Task<PagedList<TrainingListViewModel>> GetPaginatedTrainingViewModelsAsync(int pageNumber, int pageSize, bool random = false)
    {
        var trainingLists = await _catalogShowcaseContext.TrainingList.GetPaginatedTrainingListsAsync(pageNumber, pageSize, random);

        return new PagedList<TrainingListViewModel>(trainingLists.ToTrainingListViewModels(), new PageItem(pageNumber, pageSize), trainingLists.TotalCount);
    }

    public async Task<PagedList<TrainingListViewModel>> GetPaginatedTrainingViewModelsByTrainerIdAsync(int trainerId, int pageNumber, int pageSize)
    {
        var result = await _catalogShowcaseContext.TrainingList.Where(training => training.TrainerId == trainerId).PaginateAsync(pageNumber, pageSize);
        var trainingListViewModels = result.PaginatedItems.ToTrainingListViewModels();

        return new PagedList<TrainingListViewModel>(trainingListViewModels, new PageItem(pageNumber, pageSize), result.TotalCount);
    }
    public async Task<PagedList<TrainingListViewModel>> SearchTrainingViewModelsAsync(string? searchKeyword, int currentPage, int pageSize)
    {
        var trainings = await _catalogShowcaseContext.TrainingList.SearchPaginatedTrainingsAsync(searchKeyword, currentPage, pageSize);

        return new PagedList<TrainingListViewModel>(trainings.ToTrainingListViewModels(), new PageItem(currentPage, pageSize), trainings.TotalCount);
    }
}
