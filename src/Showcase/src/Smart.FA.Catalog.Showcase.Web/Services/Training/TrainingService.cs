using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Smart.FA.Catalog.Shared.Collections;
using Smart.FA.Catalog.Shared.Domain.Enumerations.Training;
using Smart.FA.Catalog.Showcase.Infrastructure.Data;
using Smart.FA.Catalog.Showcase.Infrastructure.Data.Extensions;
using Smart.FA.Catalog.Showcase.Web.Mappers;
using Smart.FA.Catalog.Showcase.Web.Options;
using Smart.FA.Catalog.Showcase.Web.Pages.Training.TrainingDetails;
using Smart.FA.Catalog.Showcase.Web.Pages.Training.TrainingList;

namespace Smart.FA.Catalog.Showcase.Web.Services.Training;

public class TrainingService : ITrainingService
{
    private readonly CatalogShowcaseContext _catalogShowcaseContext;

    private readonly MinIOOptions _minIoSettings;

    public TrainingService(CatalogShowcaseContext catalogShowcaseContext, IOptions<MinIOOptions> minIOSettings)
    {
        _catalogShowcaseContext = catalogShowcaseContext;
        _minIoSettings = minIOSettings.Value ?? throw new InvalidOperationException($"{nameof(minIOSettings)} options not defined");
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

    public async Task<PagedList<TrainingListViewModel>> SearchTrainingViewModelsByTopicIdAsync(int? searchTopicId, int currentPage, int pageSize)
    {
        var trainings = await _catalogShowcaseContext.TrainingList.SearchPaginatedTrainingsByTopicAsync(searchTopicId, currentPage, pageSize);

        return new PagedList<TrainingListViewModel>(trainings.ToTrainingListViewModels(), new PageItem(currentPage, pageSize), trainings.TotalCount);
    }

    public async Task<TrainingDetailsViewModel?> GetTrainingDetailsViewModelsByIdAsync(int trainingId)
    {
        var trainingDetails = await _catalogShowcaseContext.TrainingDetails.Where(training => training.Id == trainingId).ToListAsync();

        if (!trainingDetails.Any())
        {
            return null;
        }

        return MapTrainingDetails(trainingDetails);
    }

    private TrainingDetailsViewModel MapTrainingDetails(List<Domain.Models.TrainingDetails> trainingDetails)
    {
        if (trainingDetails is null)
        {
            return null;
        }

        var firstTrainerDetail = trainingDetails.FirstOrDefault();
        return new TrainingDetailsViewModel
        {
            Id = firstTrainerDetail.Id,
            TrainingTitle = firstTrainerDetail.TrainingTitle,
            Goal = firstTrainerDetail.Goal ?? string.Empty,
            Methodology = firstTrainerDetail.Methodology ?? string.Empty,
            PracticalModalities = firstTrainerDetail.PracticalModalities ?? string.Empty,
            TrainerFirstName = firstTrainerDetail.TrainerFirstName,
            TrainerLastName = firstTrainerDetail.TrainerLastName,
            TrainerId = firstTrainerDetail.TrainerId,
            TrainerTitle = firstTrainerDetail.TrainerTitle,
            Status = TrainingStatusType.FromValue(firstTrainerDetail.StatusId),
            Topics = trainingDetails.Select(x => Topic.FromValue(x.TrainingTopicId)).ToList(),
            Languages = trainingDetails.Select(x => x.Language).Distinct().ToList(),
            TrainerProfileImageUrl = _minIoSettings.GenerateMinIoTrainerProfileUrl(firstTrainerDetail.ProfileImagePath)
        };
    }

}
