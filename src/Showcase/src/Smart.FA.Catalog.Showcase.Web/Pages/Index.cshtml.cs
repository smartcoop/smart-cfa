using Microsoft.AspNetCore.Mvc.RazorPages;
using Smart.FA.Catalog.Shared.Collections;
using Smart.FA.Catalog.Showcase.Infrastructure.Data;
using Smart.FA.Catalog.Showcase.Web.Pages.Training.TrainingList;
using Smart.FA.Catalog.Showcase.Web.Services.Trainer;
using Smart.FA.Catalog.Showcase.Web.Services.Training;

namespace Smart.FA.Catalog.Showcase.Web.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly ITrainingService _trainingService;
    private readonly ITrainerService _trainerService;

    public int TrainersCount { get; set; }

    public int TrainingsCount { get; set; }

    public PagedList<TrainingListViewModel> Trainings { get; set; } = null!;

    public IndexModel(ILogger<IndexModel> logger, ITrainingService trainingService, ITrainerService trainerService)
    {
        _logger = logger;
        _trainingService = trainingService;
        _trainerService = trainerService;
    }

    public async Task<PageResult> OnGetAsync()
    {
        Trainings = await _trainingService.GetPaginatedTrainingViewModelsAsync(1, 10, true);
        TrainingsCount = Trainings.TotalCount;
        TrainersCount = await _trainerService.TotalCountAsync();

        return Page();
    }
}
