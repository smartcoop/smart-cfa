#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Smart.FA.Catalog.Shared.Domain.Enumerations.Training;

namespace Smart.FA.Catalog.Showcase.Web.Pages.Training.TrainingDetails;

public class TrainingDetailsModel : PageModel
{
    private readonly Infrastructure.Data.CatalogShowcaseContext _context;

    public TrainingDetailsModel(Infrastructure.Data.CatalogShowcaseContext context)
    {
        _context = context;
    }

    public TrainingDetailsViewModel Training { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            TempData["errorMessage"] = "Cette id est null";
            return RedirectToPage("/404");
        }

        var trainingDetails = await _context.TrainingDetails.Where(m => m.TrainingId == id).ToListAsync();

        if (!trainingDetails.Any())
        {
            TempData["errorMessage"] = "cette formation n'existe pas!";
            return RedirectToPage("/404");
        }

        Training = MapTrainingDetails(trainingDetails);
        return Page();
    }

    private TrainingDetailsViewModel MapTrainingDetails(List<Domain.Models.TrainingDetails> trainingDetails)
    {
        if (trainingDetails is null)
        {
            return null;
        }

        var firstLine = trainingDetails.FirstOrDefault();
        return new TrainingDetailsViewModel
        {
            TrainingId = firstLine.TrainingId,
            TrainingGoal = firstLine.TrainingGoal,
            TrainingMethodology = firstLine.TrainingMethodology,
            TrainerFirstName = firstLine.TrainerFirstName,
            TrainerLastName = firstLine.TrainerLastName,
            TrainingStatus = TrainingStatus.FromValue<TrainingStatus>(firstLine.TrainingStatusId),
            Topics = trainingDetails.Select(x => TrainingTopic.FromValue<TrainingTopic>(x.TrainingTopicId)).ToList(),
            TrainingLanguages = trainingDetails.Select(x => x.TrainingLanguage).Distinct().ToList()
        };
    }
}
