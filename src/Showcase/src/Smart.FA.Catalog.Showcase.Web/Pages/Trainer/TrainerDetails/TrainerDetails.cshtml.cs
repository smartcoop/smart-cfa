#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Smart.FA.Catalog.Shared.Collections;
using Smart.FA.Catalog.Shared.Domain.Enumerations.Training;
using Smart.FA.Catalog.Showcase.Infrastructure.Data;
using Smart.FA.Catalog.Showcase.Web.Mappers;
using Smart.FA.Catalog.Showcase.Web.Pages.Training.TrainingList;

namespace Smart.FA.Catalog.Showcase.Web.Pages.Trainer.TrainerDetails;

public class TrainerDetailsModel : PageModelBase
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
            return RedirectToNotFound("Page not found", "There is no record for this request.");
        }

        var trainerDetails = await _context.TrainerDetails.Where(trainer => trainer.Id == id).ToListAsync();

        if (!trainerDetails.Any())
        {
            return RedirectToNotFound("Page not found", "This trainer does not exist.");
        }

        if (CurrentPage <= 0)
        { 
            return RedirectToNotFound("Page not found", "The page you have requested does not exist.");
        }

        var offset = (CurrentPage - 1) * ItemsPerPage;
        var trainingIdQuery = _context.TrainingList
            .Where(training => training.TrainerId == id && training.Status == TrainingStatusType.Validated.Id)
            .Select(training => training.Id)
            .Distinct();
        var totalTrainerTrainings = await trainingIdQuery.CountAsync();

        var paginatedIds = trainingIdQuery.OrderBy(trainingId => trainingId).Skip(offset).Take(ItemsPerPage);
        var trainerTrainingList = await _context.TrainingList.Where(training => paginatedIds.Contains(training.Id)).ToListAsync();

        Trainer = MapTrainerDetails(trainerDetails, trainerTrainingList, totalTrainerTrainings);
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
