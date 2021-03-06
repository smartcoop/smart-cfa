using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Smart.Design.Razor.TagHelpers.Alert;
using Smart.FA.Catalog.Application.UseCases.Queries;
using Smart.FA.Catalog.Core.Domain.Models;
using Smart.FA.Catalog.Core.Services;
using Smart.FA.Catalog.Web.Options;

namespace Smart.FA.Catalog.Web.Pages.Admin.Trainings.Update;

public class UpdateModel : AdminPage
{
    private readonly UrlOptions _urlOptions;

    private int TrainingId { get; set; }

    public IUserIdentity UserIdentity { get; }

    [BindProperty] public UpdateTrainingViewModel UpdateTrainingViewModel { get; set; } = null!;

    public UpdateModel(IMediator mediator, IUserIdentity userIdentity, IOptions<UrlOptions> urlOptions) : base(mediator)
    {
        UserIdentity = userIdentity;
        _urlOptions = urlOptions.Value;
    }

    private void Init()
    {
        SetSideMenuItem();
    }

    public async Task<ActionResult> OnGetAsync(int id)
    {
        var user = (HttpContext.User.Identity as CustomIdentity)!;
        TrainingId = id;
        var response = await Mediator.Send(new GetTrainingByIdRequest {TrainingId = TrainingId});

        // We need to check if Training is not null otherwise MapToGetResponse will throw an exception.
        if (response.Training is null)
        {
            return RedirectToNotFound(CatalogResources.TrainingDoesNotExist);
        }

        UpdateTrainingViewModel = response.MapToViewModel(user.Trainer.DefaultLanguage);
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

        var request = UpdateTrainingViewModel.MapToRequest(UserIdentity.CurrentTrainer.DefaultLanguage.Value, id, UserIdentity.Identity.Trainer.Id);
        var response = await Mediator.Send(request);
        UpdateTrainingViewModel = response.MapToViewModel(UserIdentity.CurrentTrainer.DefaultLanguage);

        TempData.AddGlobalAlertMessage(CatalogResources.TrainingEditedWithSuccess, AlertStyle.Success);

        if (!UpdateTrainingViewModel.IsDraft)
        {
            TempData["Url"] = _urlOptions.GetShowcaseTrainingDetailsUrl(id);
        }
        else
        {
            TempData["IsDraft"] = CatalogResources.ChangeStatusToDraft;
        }

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
