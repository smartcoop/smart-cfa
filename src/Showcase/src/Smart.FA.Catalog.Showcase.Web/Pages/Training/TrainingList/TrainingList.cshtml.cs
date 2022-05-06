#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Smart.FA.Catalog.Shared.Collections;
using Smart.FA.Catalog.Shared.Domain.Enumerations.Training;
using Smart.FA.Catalog.Showcase.Web.Mappers;

namespace Smart.FA.Catalog.Showcase.Web.Pages.Training.TrainingList;

public class TrainingListModel : PageModel
{
    private readonly Infrastructure.Data.CatalogShowcaseContext _context;

    public PagedList<TrainingListViewModel> Trainings { get; set; }

    [BindProperty(SupportsGet = true)] public int CurrentPage { get; set; } = 1;

    const int ItemsPerPage = 5;

    public TrainingListModel(Infrastructure.Data.CatalogShowcaseContext context)
    {
        _context = context;
    }

    public async Task<PageResult> OnGetAsync()
    {
        var offset = (CurrentPage - 1) * ItemsPerPage;
        var count = await _context.TrainingList.AsAsyncEnumerable().GroupBy(training => training.Id).CountAsync();
        var trainingList = await _context.TrainingList
                                         .Where(training => training.Status == TrainingStatusType.Validated.Id)
                                         .AsAsyncEnumerable()
                                         .GroupBy(training => training.Id)
                                         .Skip(offset)
                                         .Take(ItemsPerPage)
                                         .SelectMany(trainingList => trainingList)
                                         .ToListAsync();

        var trainingListViewModel = trainingList.ToTrainingListViewModels();

        var pageItem = new PageItem(CurrentPage, ItemsPerPage);

        Trainings = new PagedList<TrainingListViewModel>(trainingListViewModel, pageItem, count);

        return Page();
    }
}
