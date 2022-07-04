using Microsoft.AspNetCore.Mvc;
using Smart.FA.Catalog.Shared.Collections;
using Smart.FA.Catalog.Showcase.Web.Services.Trainer;
using Smart.Design.Razor.TagHelpers.Pagination;

namespace Smart.FA.Catalog.Showcase.Web.Pages.Trainer.TrainerList;

public class TrainerListModel : PageModelBase
{
    private readonly ITrainerService _trainerService;

    public PagedList<TrainerListViewModel> Trainers { get; set; } = null!;

    [BindProperty(SupportsGet = true)]
    public int CurrentPage { get; set; } = 1;

    public PaginationSettings PaginationSettings { get; set; } = null!;

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
        SetPaginationSettings(SearchKeyword);
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
