using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Smart.Design.Razor.TagHelpers.Alert;
using Smart.FA.Catalog.Application.UseCases.Commands;
using Smart.FA.Catalog.Application.UseCases.Queries;
using Smart.FA.Catalog.Core.Domain;
using Smart.FA.Catalog.Core.Services;

namespace Smart.FA.Catalog.Web.Pages.SuperUser.Trainers;

public class List : PageModel
{
    public IUserIdentity UserIdentity { get; }

    public IMediator Mediator { get; }

    public IEnumerable<Trainer> TrainerList { get; set; }

    public List(IUserIdentity userIdentity, IMediator mediator)
    {
        UserIdentity = userIdentity;
        Mediator = mediator;
    }

    public async Task OnGetAsync()
    {
        //Get All trainers
        var getTrainerListRequest = new GetTrainerListRequest();
        var response = await Mediator.Send(getTrainerListRequest);
        TrainerList = ExcludeSelfInTrainerList(response.Trainers);
    }

    private IEnumerable<Trainer> ExcludeSelfInTrainerList(List<Trainer> trainers)
    {
        return trainers.Where(trainer => trainer.Id != UserIdentity.Id);
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
