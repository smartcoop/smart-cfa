#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Smart.FA.Catalog.Shared.Collections;
using Smart.FA.Catalog.Shared.Domain.Enumerations.Training;
using Smart.FA.Catalog.Showcase.Infrastructure.Data;
using Smart.FA.Catalog.Showcase.Web.Mappers;
using Smart.FA.Catalog.Showcase.Web.Pages.Training.TrainingList;

namespace Smart.FA.Catalog.Showcase.Web.Pages.Trainer.TrainerDetails;

public class TrainerDetailsModel : PageModel
{
    private readonly CatalogShowcaseContext _context;

    public TrainerDetailsViewModel Trainer { get; set; } = null!;

    [BindProperty(SupportsGet = true)] public int CurrentPage { get; set; } = 1;

    const int ItemsPerPage = 5;

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

        var offset = (CurrentPage - 1) * ItemsPerPage;
        var count = await _context.TrainingList
            .AsAsyncEnumerable()
            .Where(trainerTraining => trainerTraining.TrainerId == id)
            .GroupBy(trainerTraining => trainerTraining.Id)
            .CountAsync();

        var trainerTrainingList = await _context.TrainingList
            .Where(trainerTraining => trainerTraining.Status == TrainingStatusType.Validated.Id && trainerTraining.TrainerId == id)
            .AsAsyncEnumerable()
            .GroupBy(t => t.Id)
            .Skip(offset)
            .Take(ItemsPerPage)
            .SelectMany(t => t)
            .ToListAsync();


        Trainer = MapTrainerDetails(trainerDetails, trainerTrainingList, count);
        return Page();
    }

    private TrainerDetailsViewModel MapTrainerDetails(List<Domain.Models.TrainerDetails> trainerDetails,
        List<Domain.Models.TrainingList> trainerTrainingList, int count)
    {
        var firstLine = trainerDetails.FirstOrDefault();

        if (firstLine is null)
        {
            return new TrainerDetailsViewModel();
        }

        var trainingList = trainerTrainingList.ToTrainingListViewModels();
        var pageItem = new PageItem(CurrentPage, ItemsPerPage);
        var trainer = new TrainerDetailsViewModel
        {
            Id = firstLine.Id,
            Biography = firstLine.Biography,
            FirstName = firstLine.FirstName,
            LastName = firstLine.LastName,
            Title = firstLine.Title,
            //ProfileImagePath = firstLine.ProfileImagePath,
            SocialNetworks = trainerDetails.ToTrainerSocialNetworks(),
            Trainings = new PagedList<TrainingListViewModel>(trainingList, pageItem, count)
        };

        return trainer;
    }
}
