using Microsoft.AspNetCore.Mvc;
using Smart.FA.Catalog.Shared.Collections;
using Smart.FA.Catalog.Showcase.Web.Services.Trainer;
using Microsoft.Extensions.Options;
using Smart.Design.Razor.TagHelpers.Pagination;
using Smart.FA.Catalog.Showcase.Web.Options;

namespace Smart.FA.Catalog.Showcase.Web.Pages.Trainer.TrainerList;

public class TrainerListModel : PageModelBase
{
    private readonly ITrainerService _trainerService;

    public PagedList<TrainerListViewModel> Trainers { get; set; } = null!;

    [BindProperty(SupportsGet = true)]
    public int CurrentPage { get; set; } = 1;

    public PaginationSettings PaginationSettings { get; set; } = null!;

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
        SetPaginationSettings(searchKeyword);
        return Page();
    }

    private void SetPaginationSettings(string? searchKeyword)
    {
        PaginationSettings = new PaginationSettings()
        {
            NumberOfLinks = 5,
            PageNumber = CurrentPage,
            PageNumberParameterName = nameof(CurrentPage),
            TotalPages = Trainers.TotalPages,
            QueryString = !string.IsNullOrWhiteSpace(searchKeyword) ? $"{nameof(searchKeyword)}={searchKeyword}" : string.Empty
        };
    }
}
