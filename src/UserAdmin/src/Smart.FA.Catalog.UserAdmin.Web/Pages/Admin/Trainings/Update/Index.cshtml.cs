using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Smart.Design.Razor.TagHelpers.Alert;
using Smart.FA.Catalog.UserAdmin.Application.UseCases.Queries;
using Smart.FA.Catalog.UserAdmin.Domain.Domain.Models;
using Smart.FA.Catalog.UserAdmin.Domain.Services;
using Smart.FA.Catalog.UserAdmin.Web.Extensions;

namespace Smart.FA.Catalog.UserAdmin.Web.Pages.Admin.Trainings.Update;

public class UpdateModel : AdminPage
{
    private int TrainingId { get; set; }

    public List<string> ValidationErrors { get; private set; } = new();

    public IUserIdentity UserIdentity { get; }

    [BindProperty] public UpdateTrainingViewModel UpdateTrainingViewModel { get; set; } = null!;

    public UpdateModel(IMediator mediator, IUserIdentity userIdentity) : base(mediator)
    {
        UserIdentity = userIdentity;
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
            return RedirectToNotFound(CatalogResources.TrainingDoesNotExist);
        }

        UpdateTrainingViewModel = response.MapGetToResponse(user.Trainer.DefaultLanguage);
        Init();

        // Used for returning to previous page after save.
        TempData[HeaderNames.Referer] = Request.Headers["Referer"].ToString();

        return Page();
    }

    public async Task<IActionResult> OnPostUpdateAsync(int id)
    {
        if (!ModelState.IsValid)
        {
            Init();
            return Page();
        }

        var request = UpdateTrainingViewModel.MapToUpdateRequest(UserIdentity.Identity.Trainer.DefaultLanguage.Value, id, UserIdentity.Identity.Trainer.Id);
        var response = await Mediator.Send(request);
        UpdateTrainingViewModel = response.MapUpdateToResponse(UserIdentity.Identity.Trainer.DefaultLanguage);

        TempData.AddGlobalBannerMessage(CatalogResources.TrainingEditedWithSuccess, AlertStyle.Success);

        return RedirectAfterSuccessfulUpdate();
    }

    private IActionResult RedirectAfterSuccessfulUpdate()
    {
        // If the referer is another URL than the request we an redirect to that page.
        // This allows super users to be redirected to their training list and regular user to the training list.
        var returnUrl = TempData[HeaderNames.Referer]?.ToString();

        if (!string.IsNullOrWhiteSpace(returnUrl) &&
            !returnUrl.Contains(Request.Path.Value!, StringComparison.OrdinalIgnoreCase))
        {
            return Redirect(returnUrl);
        }

        // By Default we return to the user's training list.
        return RedirectToPage("/Admin/Trainings/List/Index");
    }

    protected override SideMenuItem GetSideMenuItem()
    {
        return SideMenuItem.MyTrainings;
    }
}
