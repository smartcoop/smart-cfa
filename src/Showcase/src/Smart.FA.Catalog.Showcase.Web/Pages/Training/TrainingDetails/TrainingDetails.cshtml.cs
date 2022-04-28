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
            //TO DO: add translated custom messages
            TempData["errorMessage"] = "There is no record for this request. Please try again.";
            return RedirectToPage("/404");
        }

        var trainingDetails = await _context.TrainingDetails.Where(m => m.TrainingId == id).ToListAsync();

        if (!trainingDetails.Any())
        {
            //TO DO: add translated custom messages
            TempData["errorMessage"] = "This training does not exist! Please try again.";
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
            TrainingTitle = firstLine.TrainingTitle,
            TrainingGoal = firstLine.TrainingGoal,
            TrainingMethodology = firstLine.TrainingMethodology,
            TrainingPracticalModalities = firstLine.TrainingPracticalModalities,
            TrainerFirstName = firstLine.TrainerFirstName,
            TrainerLastName = firstLine.TrainerLastName,
            TrainerTitle = firstLine.TrainerTitle,
            TrainingStatus = TrainingStatus.FromValue<TrainingStatus>(firstLine.TrainingStatusId),
            Topics = trainingDetails.Select(x => TrainingTopic.FromValue<TrainingTopic>(x.TrainingTopicId)).ToList(),
            TrainingLanguages = trainingDetails.Select(x => x.TrainingLanguage).Distinct().ToList()
        };
    }
}