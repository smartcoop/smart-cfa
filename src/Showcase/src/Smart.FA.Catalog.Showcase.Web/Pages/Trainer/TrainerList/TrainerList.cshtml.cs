using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Smart.FA.Catalog.Shared.Domain.Enumerations.Training;

namespace Smart.FA.Catalog.Showcase.Web.Pages.Trainer.TrainerList;

public class TrainerListModel : PageModel
{
    private readonly Infrastructure.Data.CatalogShowcaseContext _context;

    public List<TrainerListViewModel> Trainers { get; set; } = new List<TrainerListViewModel>();

    public TrainerListModel(Infrastructure.Data.CatalogShowcaseContext context)
    {
        _context = context;
    }

    public async Task<PageResult> OnGetAsync()
    {
        var trainerList = await _context.TrainerList
            .Where(trainer => trainer.TrainingStatusTypeId == TrainingStatusType.Validated.Id)
            .OrderBy(t => t.Id)
            .Distinct()
            .ToListAsync();

        foreach (var trainer in trainerList)
        {
            Trainers.Add(new TrainerListViewModel
            {
                Id = trainer.Id,
                FirstName = trainer.FirstName,
                LastName = trainer.LastName,
                Title = trainer.Title
                //ProfileImagePath = trainer.ProfileImagePath,
            });
        }
        return Page();
    }
}
