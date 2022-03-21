using Application.UseCases.Queries;
using Core.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web.Pages.Admin.Trainings.Update;

public class UpdateModel : AdminPage
{
    private int TrainingId { get; set; }
    public List<string> ValidationErrors { get; private set; } = new();
    [BindProperty] public UpdateTrainingViewModel UpdateTrainingViewModel { get; set; } = null!;

    public UpdateModel(IMediator mediator) : base(mediator)
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
        UpdateTrainingViewModel = response.MapGetToResponse(user.Trainer.DefaultLanguage);
        Init();
    }

    public async Task<IActionResult> OnPostUpdateAsync(int id)
    {
        var user = (HttpContext.User.Identity as CustomIdentity)!;
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var request = UpdateTrainingViewModel.MapToUpdateRequest(user.Trainer.DefaultLanguage.Value, id, user.Trainer.Id);
        var response = await Mediator.Send(request);
        UpdateTrainingViewModel = response.MapUpdateToResponse(user.Trainer.DefaultLanguage);

        return RedirectToPage("/Admin/Trainings/List/Index");
    }

    protected override SideMenuItem GetSideMenuItem()
    {
        return SideMenuItem.MyTrainings;
    }
}
