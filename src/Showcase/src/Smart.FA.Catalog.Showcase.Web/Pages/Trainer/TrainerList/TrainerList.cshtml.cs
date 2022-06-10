using Microsoft.AspNetCore.Mvc;
using Smart.FA.Catalog.Shared.Collections;
using Smart.FA.Catalog.Showcase.Web.Services.Trainer;
using Microsoft.Extensions.Options;
using Smart.FA.Catalog.Showcase.Web.Options;

namespace Smart.FA.Catalog.Showcase.Web.Pages.Trainer.TrainerList;

public class TrainerListModel : PageModelBase
{
    private readonly ITrainerService _trainerService;

    public PagedList<TrainerListViewModel> Trainers { get; set; } = null!;

    [BindProperty(SupportsGet = true)]
    public int CurrentPage { get; set; } = 1;

    private const int ItemsPerPage = 8;

    public TrainerListModel(ITrainerService trainerService, IOptions<MinIOOptions> minIOOptions)
    {
        _trainerService = trainerService;
    }

    public async Task<ActionResult> OnGetAsync([FromQuery] string? searchKeyword)
    {
        if (CurrentPage <= 0)
        {
            return RedirectToNotFound();
        }

        Trainers = await _trainerService.SearchTrainerDetailsViewModelsAsync(searchKeyword, CurrentPage, ItemsPerPage);
        return Page();
    }
}
