using Application.UseCases.Commands;
using Application.UseCases.Queries;
using Core.Domain.Dto;
using Core.SeedWork;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Web.Identity;
using Web.Options;

namespace Web.Pages.Admin.Trainings.List;

public class ListModel : AdminPage
{
    private readonly CatalogContext _context;
    private readonly AdminOptions _adminOptions;
    public PagedList<TrainingDto>? Trainings { get; set; } = null!;

    [BindProperty(SupportsGet = true)] public int CurrentPage { get; set; } = 1;

    public ListModel(IMediator mediator, IOptions<AdminOptions> adminOptions, CatalogContext context) : base(mediator)
    {
        _context = context;
        _adminOptions = adminOptions.Value ?? throw new ArgumentNullException(nameof(adminOptions));
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var user = (HttpContext.User.Identity as CustomIdentity)!;
        SetSideMenuItem();
        var response = await Mediator.Send(new GetPagedTrainingsFromTrainerRequest
        {
            TrainerId = user.Trainer.Id,
            Language =  user.Trainer.DefaultLanguage,
            PageItem = new PageItem(CurrentPage, _adminOptions.Training!.NumberOfTrainingsDisplayed)
        });
        Trainings = response.Trainings;
        return Page();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var response = await Mediator.Send(new DeleteTrainingRequest {TrainingId = id});
        return RedirectToPage();
    }

    protected override SideMenuItem GetSideMenuItem() => SideMenuItem.MyTrainings;
}
