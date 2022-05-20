using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Smart.Design.Razor.TagHelpers.Alert;
using Smart.FA.Catalog.Shared.Collections;
using Smart.FA.Catalog.UserAdmin.Application.UseCases.Commands;
using Smart.FA.Catalog.UserAdmin.Application.UseCases.Queries;
using Smart.FA.Catalog.UserAdmin.Domain.Domain;
using Smart.FA.Catalog.UserAdmin.Domain.Domain.Models;
using Smart.FA.Catalog.UserAdmin.Infrastructure.Persistence;
using Smart.FA.Catalog.UserAdmin.Web.Extensions;
using Smart.FA.Catalog.UserAdmin.Web.Options;

namespace Smart.FA.Catalog.UserAdmin.Web.Pages.Admin.Trainings.List;

public class ListModel : AdminPage
{
    private readonly AdminOptions _adminOptions;
    public PagedList<Training>? Trainings { get; set; }

    [BindProperty(SupportsGet = true)] public int CurrentPage { get; set; } = 1;

    public ListModel(IMediator mediator, IOptions<AdminOptions> adminOptions, CatalogContext context) : base(mediator)
    {
        _adminOptions = adminOptions.Value ?? throw new ArgumentNullException(nameof(adminOptions));
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var user = (HttpContext.User.Identity as CustomIdentity)!;
        SetSideMenuItem();
        var response = await Mediator.Send(new GetPagedTrainingListFromTrainerRequest
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
        await Mediator.Send(new DeleteTrainingRequest { TrainingId = id });
        TempData.AddGlobalBannerMessage(CatalogResources.TrainingDeletedWithSuccess, AlertStyle.Success);
        return RedirectToPage();
    }

    protected override SideMenuItem GetSideMenuItem() => SideMenuItem.MyTrainings;
}
