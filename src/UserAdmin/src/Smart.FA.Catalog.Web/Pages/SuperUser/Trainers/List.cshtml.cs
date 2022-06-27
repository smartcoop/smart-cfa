using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Options;
using Smart.Design.Razor.TagHelpers.Alert;
using Smart.FA.Catalog.Application.UseCases.Commands;
using Smart.FA.Catalog.Application.UseCases.Queries;
using Smart.FA.Catalog.Core.Domain;
using Smart.FA.Catalog.Core.Services;
using Smart.FA.Catalog.Shared.Collections;
using Smart.FA.Catalog.Web.Options;

namespace Smart.FA.Catalog.Web.Pages.SuperUser.Trainers;

public class List : PageModel
{
    [BindProperty(SupportsGet = true)]  public string? TrainerNameSearchQuery { get; set; }

    [BindProperty(SupportsGet = true)] public int CurrentPage { get; set; } = 1;

    public IUserIdentity UserIdentity { get; }

    public IMediator Mediator { get; }

    public SuperUserOptions Settings { get; set; }

    public PagedList<Trainer> TrainerList { get; set; }

    public List(IUserIdentity userIdentity, IMediator mediator, IOptions<SuperUserOptions> superUserOptions)
    {
        UserIdentity = userIdentity;
        Mediator = mediator;
        Settings = superUserOptions.Value;
    }

    public async Task OnGetAsync()
    {
        //Get All trainers
        var getTrainerListRequest =
            new GetTrainerListRequest { TrainerName = TrainerNameSearchQuery, PageItem = new PageItem(CurrentPage, Settings.NumberOfTrainerPerPage), SelfTrainerId = UserIdentity.Id };
        TrainerList = await Mediator.Send(getTrainerListRequest);
    }

    public async Task<ActionResult> OnPostDeleteAsync(int id)
    {
        var trainerResponse = await Mediator.Send(new GetTrainerRequest { TrainerId = id });
        await Mediator.Send(new BlackListUserRequest { UserId = trainerResponse.Trainer.Identity.UserId, ApplicationTypeId = trainerResponse.Trainer.Identity.ApplicationTypeId });
        await Mediator.Send(new DeleteTrainerRequest { TrainerId = id });
        TempData.AddGlobalAlertMessage(CatalogResources.TrainerDeletedWithSuccess, AlertStyle.Success);
        return RedirectToPage();
    }
}
