using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Smart.FA.Catalog.Showcase.Infrastructure.Data;
using Smart.FA.Catalog.Showcase.Web.Mappers;

namespace Smart.FA.Catalog.Showcase.Web.Pages.Trainer.TrainerDetails;

public class TrainerDetailsModel : PageModel
{
    private readonly CatalogShowcaseContext _context;
    public TrainerDetailsViewModel Trainer { get; set; } = null!;

    public TrainerDetailsModel(CatalogShowcaseContext context)
    {
        _context = context;
    }

    public async Task<ActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            TempData["errorMessage"] = "There is no record for this request.";
            return RedirectToPage("/404");
        }

        var trainerDetails = await _context.TrainerDetails.Where(trainer => trainer.Id == id).ToListAsync();

        if (!trainerDetails.Any())
        {
            TempData["errorMessage"] = "This trainer does not exist.";
            return RedirectToPage("/404");
        }

        var trainerTrainingList = _context.TrainingList.Where(trainerTraining => trainerTraining.TrainerId == id).ToList();

        Trainer = trainerDetails.ToTrainerDetailsViewModel(trainerTrainingList);

        return Page();
    }
}
