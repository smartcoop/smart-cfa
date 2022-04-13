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
            return RedirectToPage("/Error");
        }

        var trainingDetails = await _context.TrainingDetails.Where(m => m.TrainingId == id).ToListAsync();

        if (!trainingDetails.Any())
        {
            return RedirectToPage("/Error");
        }

        Training = MapTrainingDetails(trainingDetails);
        return Page();
    }

    private TrainingDetailsViewModel MapTrainingDetails(List<Domain.Models.TrainingDetails> trainingDetails)
    {
        TrainingDetailsViewModel training = new TrainingDetailsViewModel();
        training.TrainingId = trainingDetails.FirstOrDefault().TrainingId;
        training.TrainingGoal = trainingDetails.FirstOrDefault().TrainingGoal;
        training.TrainingMethodology = trainingDetails.FirstOrDefault().TrainingMethodology;
        training.TrainingTitle = trainingDetails.FirstOrDefault().TrainingTitle;
        training.TrainerFirstName = trainingDetails.FirstOrDefault().TrainerFirstName;
        training.TrainerLastName = trainingDetails.FirstOrDefault().TrainerLastName;
        training.TrainingStatus = TrainingStatus.FromValue<TrainingStatus>(trainingDetails.FirstOrDefault().TrainingStatusId);
        training.Topics = trainingDetails.Select(x => TrainingTopic.FromValue<TrainingTopic>(x.TrainingTopicId)).ToList();
        training.TrainingLanguages = trainingDetails.Select(x => (x.TrainingLanguage)).Distinct().ToList();

        return training;
    }
}
