#nullable disable
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Smart.FA.Catalog.Shared.Domain.Enumerations.Training;
using Smart.FA.Catalog.Showcase.Web.Mappers;

namespace Smart.FA.Catalog.Showcase.Web.Pages.Training.TrainingList;

public class TrainingListModel : PageModel
{
    private readonly Infrastructure.Data.CatalogShowcaseContext _context;
    public List<TrainingListViewModel> Trainings { get; set; } = new List<TrainingListViewModel>();


    public TrainingListModel(Infrastructure.Data.CatalogShowcaseContext context)
    {
        _context = context;
    }

    public async Task<PageResult> OnGetAsync()
    {
        var trainingList = await _context.TrainingList
            .Where(training => training.Status == TrainingStatus.Validated.Id)
            .OrderBy(t => t.TrainingId)
            .ToListAsync();

        Trainings = trainingList.ToTrainingListViewModels();
        return Page();
    }
}
