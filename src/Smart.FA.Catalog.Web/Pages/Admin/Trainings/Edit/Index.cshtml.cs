using Application.UseCases.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Pages.Admin.Trainings.Edit;

public class EditModel : AdminPage
{
    public int TrainingId { get; private set; }
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
        TrainingId = id;
        var response = await Mediator.Send(new GetTrainingFromIdRequest{TrainingId = TrainingId}, CancellationToken.None);
        EditTrainingViewModel = response.MapGetToResponse("FR");
        await InitAsync();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToPage();
        }

        //TODO: change hardcoded language && trainerId to connection data
        var response = await Mediator.Send(EditTrainingViewModel.MapToUpdateRequest("FR", id, 1));
        EditTrainingViewModel = response.MapUpdateToResponse("FR");

        return RedirectToPage("/Admin/Trainings/List/Index");
    }

    protected override SideMenuItem GetSideMenuItem()
    {
        return SideMenuItem.MyTrainings;
    }
}
