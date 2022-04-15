using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Smart.FA.Catalog.Application.UseCases.Commands;
using Smart.FA.Catalog.Application.UseCases.Queries;
using Smart.FA.Catalog.Core.Domain.Dto;
using Smart.FA.Catalog.Core.Domain.Models;
using Smart.FA.Catalog.Core.SeedWork;
using Smart.FA.Catalog.Infrastructure.Persistence;
using Smart.FA.Catalog.Web.Options;

namespace Smart.FA.Catalog.Web.Pages.Admin.Trainings.List;

[Authorize(Policy = "AtLeastOneValidUserChartRevisionApproved")]
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
            Language = user.Trainer.DefaultLanguage,
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
