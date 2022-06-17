using Microsoft.AspNetCore.Mvc;
using Smart.FA.Catalog.Shared.Collections;
using Smart.FA.Catalog.Showcase.Web.Services.Trainer;

namespace Smart.FA.Catalog.Showcase.Web.Pages.Trainer.TrainerList;

public class TrainerListModel : PageModelBase
{
    private readonly ITrainerService _trainerService;

    public PagedList<TrainerListViewModel> Trainers { get; set; } = null!;

    [BindProperty(SupportsGet = true)]
    public int CurrentPage { get; set; } = 1;

    private const int ItemsPerPage = 8;

    [FromQuery(Name = nameof(SearchKeyword))]
    public string? SearchKeyword { get; set; }

    public TrainerListModel(ITrainerService trainerService)
    {
        _trainerService = trainerService;
    }

    public async Task<ActionResult> OnGetAsync()
    {
        if (CurrentPage <= 0)
        {
            return RedirectToNotFound();
        }

        Trainers = await _trainerService.SearchTrainerDetailsViewModelsAsync(SearchKeyword, CurrentPage, ItemsPerPage);
        return Page();
    }
}
