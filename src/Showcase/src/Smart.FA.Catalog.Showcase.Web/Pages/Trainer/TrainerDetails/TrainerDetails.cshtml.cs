#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Smart.FA.Catalog.Shared.Domain.Enumerations.Trainer;
using Smart.FA.Catalog.Shared.Domain.Enumerations.Training;
using Smart.FA.Catalog.Showcase.Infrastructure.Data;
using Smart.FA.Catalog.Showcase.Web.Pages.Training;

namespace Smart.FA.Catalog.Showcase.Web.Pages.Trainer.TrainerDetails;

public class TrainerDetailsModel : PageModel
{
    private readonly CatalogShowcaseContext _context;

    public TrainerDetailsModel(CatalogShowcaseContext context)
    {
        _context = context;
    }

    public TrainerDetailsViewModel Trainer { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            TempData["errorMessage"] = "There is no record for this request. Please try again.";
            return RedirectToPage("/404");
        }

        var trainerDetails = await _context.TrainerDetails.Where(trainer => trainer.Id == id).ToListAsync();

        if (!trainerDetails.Any())
        {
            TempData["errorMessage"] = "This trainer does not exist! Please try again.";
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
            TrainerId = firstLine.Id,
            TrainerBiography = firstLine.Biography,
            TrainerFirstName = firstLine.FirstName,
            TrainerLastName = firstLine.LastName,
            TrainerTitle = firstLine.Title
        };

        //Add trainer's social networks
        foreach (var detail in trainerDetails)
        {
            if (detail.SocialNetwork != null)
                trainer.TrainerSocialNetworks.Add(new TrainerSocialNetwork()
                {
                    SocialNetwork = SocialNetwork.FromValue<SocialNetwork>((int)detail.SocialNetwork),
                    SocialNetworkUrl = detail.UrlToProfile
                });
        }

        //Add trainer's trainings
        var trainerTrainingsByIds = trainerTrainingList.ToLookup(t => t.TrainingId);
        foreach (var trainerTraining in trainerTrainingsByIds)
        {
            trainer.TrainerTrainings.Add(new TrainingListViewModel()
            {
                TrainingTitle = trainerTraining.FirstOrDefault().TrainingTitle,
                Topics = trainerTraining.Select(x => TrainingTopic.FromValue<TrainingTopic>(x.TrainingTopic)).ToList()
            });
        }

        return trainer;
    }
}
