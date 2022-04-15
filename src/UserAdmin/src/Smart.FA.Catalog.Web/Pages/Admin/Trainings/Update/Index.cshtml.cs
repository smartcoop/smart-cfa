using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Smart.FA.Catalog.Application.UseCases.Queries;
using Smart.FA.Catalog.Core.Domain.Models;

namespace Smart.FA.Catalog.Web.Pages.Admin.Trainings.Update;

[Authorize(Policy = Policies.List.AtLeastOneValidUserChartRevisionApproval)]
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

    public async Task<ActionResult> OnGet(int id)
    {
        var user = (HttpContext.User.Identity as CustomIdentity)!;
        TrainingId = id;
        var response = await Mediator.Send(new GetTrainingFromIdRequest {TrainingId = TrainingId}, CancellationToken.None);

        // We need to check if Training is not null otherwise MapToGetResponse will throw an exception.
        if (response.Training is null)
        {
            return NotFound();
        }

        UpdateTrainingViewModel = response.MapGetToResponse(user.Trainer.DefaultLanguage);
        Init();

        return Page();
    }

    public async Task<IActionResult> OnPostUpdateAsync(int id)
    {
        var user = (HttpContext.User.Identity as CustomIdentity)!;
        if (!ModelState.IsValid)
        {
            Init();
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
