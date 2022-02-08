using Application.UseCases.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Web.Identity;

namespace Web.Pages.Admin.Trainings.Edit;

public class EditModel : AdminPage
{
    private int TrainingId { get; set; }
    public List<string> ValidationErrors { get; private set; } = new();
    [BindProperty] public EditTrainingViewModel EditTrainingViewModel { get; set; } = null!;

    public EditModel(IMediator mediator) : base(mediator)
    {
    }

    private void Init()
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
        Init();
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
