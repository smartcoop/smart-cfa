using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Smart.FA.Catalog.Shared.Collections;
using Smart.FA.Catalog.Shared.Domain.Enumerations.Training;

namespace Smart.FA.Catalog.Showcase.Web.Pages.Trainer.TrainerList;

public class TrainerListModel : PageModelBase
{
    private readonly Infrastructure.Data.CatalogShowcaseContext _context;

    public PagedList<TrainerListViewModel> Trainers { get; set; }

    [BindProperty(SupportsGet = true)] public int CurrentPage { get; set; } = 1;

    private const int ItemsPerPage = 5;

    public TrainerListModel(Infrastructure.Data.CatalogShowcaseContext context)
    {
        _context = context;
    }

    public async Task<ActionResult> OnGetAsync()
    {
        if (CurrentPage <= 0)
        {
            return RedirectToNotFound("Page not found", "The page you have requested does not exist.");
        }
        var offset = (CurrentPage - 1) * ItemsPerPage;

        var trainerIdQuery =  _context.TrainerList
            .Where(trainer => trainer.TrainingStatusTypeId == TrainingStatusType.Validated.Id)
            .Distinct()
            .Select(trainer => trainer.Id);

        var totalTrainers = await trainerIdQuery.CountAsync();

        var paginatedIds = trainerIdQuery.OrderBy(trainerId => trainerId)
            .Skip(offset).
            Take(ItemsPerPage);

        var trainerList = await _context.TrainerList.Where(trainer => paginatedIds.Contains(trainer.Id))
            .Distinct().
            ToListAsync();

        var trainerViewModel = trainerList.Select(trainer => new TrainerListViewModel
            {
                Id = trainer.Id,
                FirstName = trainer.FirstName,
                LastName = trainer.LastName,
                Title = trainer.Title
                //ProfileImagePath = trainer.ProfileImagePath,
            })
            .ToList();

        var pageItem = new PageItem(CurrentPage, ItemsPerPage);
        Trainers = new PagedList<TrainerListViewModel>(trainerViewModel, pageItem, totalTrainers);

        return Page();
    }
}
