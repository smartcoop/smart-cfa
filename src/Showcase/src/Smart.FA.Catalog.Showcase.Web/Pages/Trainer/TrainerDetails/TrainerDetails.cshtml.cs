#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Smart.FA.Catalog.Shared.Domain.Enumerations.Trainer;
using Smart.FA.Catalog.Shared.Domain.Enumerations.Training;
using Smart.FA.Catalog.Showcase.Infrastructure.Data;
using Smart.FA.Catalog.Showcase.Web.Pages.Training.TrainingList;

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
        if (id == null)
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

        Trainer = MapTrainerDetails(trainerDetails, trainerTrainingList);
        return Page();
    }

    private TrainerDetailsViewModel MapTrainerDetails(List<Domain.Models.TrainerDetails> trainerDetails,
                                                      List<Domain.Models.TrainingList> trainerTrainingList)
    {
        var firstLine = trainerDetails.FirstOrDefault();

        if (firstLine == null)
        {
            return new TrainerDetailsViewModel();
        }

        var trainer = new TrainerDetailsViewModel
        {
            Id = firstLine.Id,
            Biography = firstLine.Biography,
            FirstName = firstLine.FirstName,
            LastName = firstLine.LastName,
            Title = firstLine.Title
        };

        //Add trainer's social networks
        foreach (var detail in trainerDetails.Where(detail => detail.SocialNetwork is not null))
        {
            var SocialNetworkName = SocialNetwork.FromValue<SocialNetwork>((int) detail.SocialNetwork);
            trainer.SocialNetworks.Add(new TrainerSocialNetwork()
            {
                SocialNetwork = SocialNetworkName,
                SocialNetworkUrl = detail.UrlToProfile,
                IconPathFileName = $"/icons/{SocialNetworkName}.svg"
            });
        }

        //Add trainer's trainings
        var trainerTrainingsByIds = trainerTrainingList.ToLookup(t => t.TrainingId);
        foreach (var trainerTraining in trainerTrainingsByIds)
        {
            trainer.Trainings.Add(new TrainingListViewModel()
            {
                TrainingId = trainerTraining.FirstOrDefault().TrainingId,
                Title = trainerTraining.FirstOrDefault().Title,
                Topics = trainerTraining.Select(x => TrainingTopic.FromValue<TrainingTopic>(x.Topic)).ToList()
            });
        }

        return trainer;
    }
}
