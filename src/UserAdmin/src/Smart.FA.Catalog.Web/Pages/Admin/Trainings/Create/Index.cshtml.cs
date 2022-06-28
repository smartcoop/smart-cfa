using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Options;
using Smart.Design.Razor.TagHelpers.Alert;
using Smart.FA.Catalog.Core.Services;
using Smart.FA.Catalog.Web.Options;

namespace Smart.FA.Catalog.Web.Pages.Admin.Trainings.Create;

public class CreateModel : AdminPage
{
    private readonly UrlOptions _urlOptions;

    public IUserIdentity UserIdentity { get; }

    [BindProperty] public CreateTrainingViewModel CreateTrainingViewModel { get; set; } = new();

    public CreateModel(IMediator mediator, IUserIdentity userIdentity, IOptions<UrlOptions> urlOptions) : base(mediator)
    {
        UserIdentity = userIdentity;
        _urlOptions = urlOptions.Value ?? throw new ArgumentException($"{urlOptions} not found");
    }

    private void Init()
    {
        SetSideMenuItem();
    }

    public void OnGet()
    {
        Init();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            Init();
            return Page();
        }

        var createTrainingRequest = CreateTrainingViewModel.MapToRequest(UserIdentity.CurrentTrainer.Id, UserIdentity.CurrentTrainer.DefaultLanguage);
        var response = await Mediator.Send(createTrainingRequest);

        TempData.AddGlobalAlertMessage(CatalogResources.TrainingCreatedWithSuccess, AlertStyle.Success);
        if (!createTrainingRequest.IsDraft)
        {
            TempData["Url"] = _urlOptions.GetShowcaseTrainingDetailsUrl(response.TrainingId);
        }

        return RedirectToPage("/Admin/Trainings/List/Index");
    }

    protected override SideMenuItem GetSideMenuItem()
    {
        return SideMenuItem.MyTrainings;
    }
}
