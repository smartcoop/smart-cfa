using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Options;
using Smart.Design.Razor.TagHelpers.Alert;
using Smart.Design.Razor.TagHelpers.Pagination;
using Smart.FA.Catalog.Application.UseCases.Commands;
using Smart.FA.Catalog.Application.UseCases.Queries;
using Smart.FA.Catalog.Core.Domain;
using Smart.FA.Catalog.Core.Domain.Models;
using Smart.FA.Catalog.Shared.Collections;
using Smart.FA.Catalog.Web.Options;

namespace Smart.FA.Catalog.Web.Pages.Admin.Trainings.List;

public class ListModel : AdminPage
{
    private readonly AdminOptions _adminOptions;
    public PagedList<Training>? Trainings { get; set; }

    [BindProperty(SupportsGet = true)] public int CurrentPage { get; set; } = 1;

    public PaginationSettings PaginationSettings { get; set; } = null!;

    public ListModel(IMediator mediator, IOptions<AdminOptions> adminOptions) : base(mediator)
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

        SetPaginationSettings();

        return Page();
    }

    private void SetPaginationSettings()
    {
        PaginationSettings = new PaginationSettings()
        {
            NumberOfLinks = 5,
            PageNumber = Trainings!.CurrentPage,
            TotalPages = Trainings.TotalPages,
            PageNumberParameterName = nameof(CurrentPage)
        };
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        await Mediator.Send(new DeleteTrainingRequest { TrainingId = id });
        TempData.AddGlobalAlertMessage(CatalogResources.TrainingDeletedWithSuccess, AlertStyle.Success);
        return RedirectToPage();
    }

    protected override SideMenuItem GetSideMenuItem() => SideMenuItem.MyTrainings;
}
