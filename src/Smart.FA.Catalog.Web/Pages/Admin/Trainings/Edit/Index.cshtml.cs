using Api.Extensions;
using Api.Identity;
using Application.UseCases.Queries;
using Core.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Pages.Admin.Trainings.Edit;

public class EditModel : AdminPage
{
    public int TrainingId { get; private set; }
    public List<string> ValidationErrors { get; set; } = new();
    [BindProperty] public EditTrainingViewModel EditTrainingViewModel { get; set; } = new();

    public EditModel(IMediator mediator) : base(mediator)
    {
    }

    private async Task InitAsync()
    {
        SetSideMenuItem();
    }

    public async Task OnGet(int id)
    {
        var user = (HttpContext.User.Identity as CustomIdentity)!;
        TrainingId = id;
        var response =
            await Mediator.Send(new GetTrainingFromIdRequest {TrainingId = TrainingId}, CancellationToken.None);
        EditTrainingViewModel = response.MapGetToResponse(user.Trainer.DefaultLanguage);
        await InitAsync();
    }

    public async Task<IActionResult> OnPostUpdateAsync(int id)
    {
        var user = (HttpContext.User.Identity as CustomIdentity)!;
        if (!ModelState.IsValid)
        {
            return RedirectToPage();
        }

        var response =
            await Mediator.Send(
                EditTrainingViewModel.MapToUpdateRequest(user.Trainer.DefaultLanguage.Value, id, user.Trainer.Id));
        EditTrainingViewModel = response.MapUpdateToResponse(user.Trainer.DefaultLanguage);

        return RedirectToPage("/Admin/Trainings/List/Index");
    }

    public async Task<IActionResult> OnPostValidateModelAsync()
    {
        var user = (HttpContext.User.Identity as CustomIdentity)!;
        var response =
            await Mediator.Send(
                EditTrainingViewModel.MapDraftToRequest(user.Trainer.Id, user.Trainer.DefaultLanguage));
        ValidationErrors = response.ValidationErrors;
        return Page();
    }


    protected override SideMenuItem GetSideMenuItem()
    {
        return SideMenuItem.MyTrainings;
    }
}
