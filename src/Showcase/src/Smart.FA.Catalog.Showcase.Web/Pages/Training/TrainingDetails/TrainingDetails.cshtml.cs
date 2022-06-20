#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Smart.FA.Catalog.Shared.Domain.Enumerations.Training;

namespace Smart.FA.Catalog.Showcase.Web.Pages.Training.TrainingDetails;

public class TrainingDetailsModel : PageModelBase
{
    private readonly Infrastructure.Data.CatalogShowcaseContext _context;

    public TrainingDetailsViewModel Training { get; set; }

    public TrainingDetailsModel(Infrastructure.Data.CatalogShowcaseContext context)
    {
        _context = context;
    }

    public async Task<ActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return RedirectToNotFound();
        }

        var trainingDetails = await _context.TrainingDetails.Where(training => training.Id == id).ToListAsync();

        if (!trainingDetails.Any())
        {
            return RedirectToNotFound(message: ShowcaseResources.TrainingWasNotFound);
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
            Id = firstLine.Id,
            TrainingTitle = firstLine.TrainingTitle,
            Goal = firstLine.Goal,
            Methodology = firstLine.Methodology,
            PracticalModalities = firstLine.PracticalModalities,
            TrainerFirstName = firstLine.TrainerFirstName,
            TrainerLastName = firstLine.TrainerLastName,
            TrainerId = firstLine.TrainerId,
            TrainerTitle = firstLine.TrainerTitle,
            Status = TrainingStatusType.FromValue(firstLine.StatusId),
            Topics = trainingDetails.Select(x => Topic.FromValue(x.TrainingTopicId)).ToList(),
            Languages = trainingDetails.Select(x => x.Language).Distinct().ToList()
        };
    }
}
