#nullable disable
using Microsoft.AspNetCore.Mvc;
using Smart.FA.Catalog.Shared.Collections;
using Smart.FA.Catalog.Showcase.Web.Services.Training;

namespace Smart.FA.Catalog.Showcase.Web.Pages.Training.TrainingList;

public class TrainingListModel : PageModelBase
{
    private readonly ITrainingService _trainingService;

    public PagedList<TrainingListViewModel> Trainings { get; set; }

    [BindProperty(SupportsGet = true)] public int CurrentPage { get; set; } = 1;

    const int ItemsPerPage = 5;

   public TrainingListModel(ITrainingService trainingService)
    {
        _trainingService = trainingService;
    }
    public async Task<ActionResult> OnGetAsync([FromQuery] string? searchKeyword)
    {
        if (CurrentPage <= 0)
        {
            return RedirectToNotFound();
        }

        Trainings = await _trainingService.SearchTrainingViewModelsAsync(searchKeyword, CurrentPage, ItemsPerPage);
        return Page();
    }

    public async Task<ActionResult> OnGetSearchTopicAsync(int? id)
    {
        if (CurrentPage <= 0)
        {
            return RedirectToNotFound();
        }

        Trainings = await _trainingService.SearchTrainingByTopicViewModelsAsync(id, CurrentPage, ItemsPerPage);
        return Page();
    }
}
