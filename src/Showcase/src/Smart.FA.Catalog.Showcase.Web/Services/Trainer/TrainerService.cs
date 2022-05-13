using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Smart.FA.Catalog.Shared.Collections;
using Smart.FA.Catalog.Showcase.Infrastructure.Data;
using Smart.FA.Catalog.Showcase.Infrastructure.Data.Extensions;
using Smart.FA.Catalog.Showcase.Web.Mappers;
using Smart.FA.Catalog.Showcase.Web.Options;
using Smart.FA.Catalog.Showcase.Web.Pages.Trainer.TrainerDetails;
using Smart.FA.Catalog.Showcase.Web.Pages.Trainer.TrainerList;
using Smart.FA.Catalog.Showcase.Web.Pages.Training.TrainingList;

namespace Smart.FA.Catalog.Showcase.Web.Services.Trainer;

public class TrainerService : ITrainerService
{
    private readonly CatalogShowcaseContext _catalogShowcaseContext;
    private readonly MinIOOptions _minIoSettings;

    public TrainerService(CatalogShowcaseContext catalogShowcaseContext, IOptions<MinIOOptions> minIOSettings)
    {
        _catalogShowcaseContext = catalogShowcaseContext;
        _minIoSettings = minIOSettings.Value ?? throw new InvalidOperationException($"MinIO options not defined");
    }

    /// <inheritdoc />
    public async Task<TrainerDetailsViewModel?> GetTrainerDetailsViewModelsByIdAsync(int trainerId, int pageNumber, int pageSize)
    {
        var trainerDetails = await _catalogShowcaseContext.TrainerDetails.FirstOrDefaultAsync(trainer => trainer.Id == trainerId);
        if (trainerDetails is null)
        {
            return null;
        }

        var paginatedTrainingLists = await _catalogShowcaseContext.TrainingList.GetPaginatedTrainingListsByTrainerIdAsync(trainerId, pageNumber, pageSize);

        return new TrainerDetailsViewModel
        {
            Id = trainerDetails.Id,
            Biography = trainerDetails.Biography,
            FirstName = trainerDetails.FirstName,
            LastName = trainerDetails.LastName,
            Title = trainerDetails.Title,
            Trainings = new PagedList<TrainingListViewModel>(paginatedTrainingLists.ToTrainingListViewModels(), new(pageNumber, pageSize), paginatedTrainingLists.TotalCount),
            SocialNetworks = await GetTrainerSocialNetworksAsync(trainerId),
            ProfileImageUrl = _minIoSettings.GenerateMinIoTrainerProfileUrl(trainerDetails.ProfileImagePath)
        };
    }

    public async Task<PagedList<TrainerListViewModel>> SearchTrainerDetailsViewModelsAsync(string? searchKeyword, int currentPage, int pageSize)
    {
        var trainers = await _catalogShowcaseContext.TrainerList.SearchPaginatedTrainersAsync(searchKeyword, currentPage, pageSize);

        return new PagedList<TrainerListViewModel>(trainers.ToTrainerListViewModels(_minIoSettings), new PageItem(currentPage, pageSize), trainers.TotalCount);
    }

    private async Task<List<TrainerSocialNetwork>> GetTrainerSocialNetworksAsync(int trainerId)
    {
        var socialNetworksViewModels = await _catalogShowcaseContext.TrainerDetails
            .Where(trainer => trainer.SocialNetwork != null && !string.IsNullOrEmpty(trainer.UrlToProfile))
            .Where(trainer => trainer.Id == trainerId)
            .OrderByDescending(trainer => trainer.SocialNetwork)
            .Select(trainer => trainer.ToTrainerSocialNetwork())
            .ToListAsync();

        return socialNetworksViewModels;
    }

    /// <inheritdoc />
    public Task<int> TotalCountAsync()
    {
        return _catalogShowcaseContext.TrainerDetails.TrainerCountAsync();
    }
}
