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
    private readonly ILogger<CreateModel> _logger;
    private readonly UrlOptions _urlOptions;

    public IUserIdentity UserIdentity { get; }

    [BindProperty] public CreateTrainingViewModel CreateTrainingViewModel { get; set; } = new();

    public string ShowcaseTrainingDetailsUrl { get; set; }

    public CreateModel(IMediator mediator, ILogger<CreateModel> logger, IUserIdentity userIdentity, IOptions<UrlOptions> urlOptions) : base(mediator)
    {
        _logger = logger;
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

        var request = CreateTrainingViewModel.MapToRequest(UserIdentity.CurrentTrainer.Id, UserIdentity.CurrentTrainer.DefaultLanguage);
        var reponse = await Mediator.Send(request);

        TempData.AddGlobalAlertMessage(CatalogResources.TrainingCreatedWithSuccess, AlertStyle.Success);
        if (!request.IsDraft)
        {
            TempData["Url"] = _urlOptions.GetShowcaseTrainingDetailsUrl(reponse.TrainingId);
        }

        return RedirectToPage("/Admin/Trainings/List/Index");
    }

    protected override SideMenuItem GetSideMenuItem()
    {
        return SideMenuItem.MyTrainings;
    }
}
