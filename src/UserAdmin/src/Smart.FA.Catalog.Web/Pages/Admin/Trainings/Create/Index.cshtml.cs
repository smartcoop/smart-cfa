using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Smart.Design.Razor.TagHelpers.Alert;
using Smart.FA.Catalog.Core.Services;

namespace Smart.FA.Catalog.Web.Pages.Admin.Trainings.Create;

public class CreateModel : AdminPage
{
    private readonly ILogger<CreateModel> _logger;

    public IUserIdentity UserIdentity { get; }

    [BindProperty] public CreateTrainingViewModel CreateTrainingViewModel { get; set; } = new();

    public List<string> ValidationErrors { get; set; } = new();

    public CreateModel(IMediator mediator, ILogger<CreateModel> logger, IUserIdentity userIdentity) : base(mediator)
    {
        _logger = logger;
        UserIdentity = userIdentity;
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
        await Mediator.Send(request);

        TempData.AddGlobalBannerMessage(CatalogResources.TrainingCreatedWithSuccess, AlertStyle.Success);
        
        return RedirectToPage("/Admin/Trainings/List/Index");
    }

    protected override SideMenuItem GetSideMenuItem()
    {
        return SideMenuItem.MyTrainings;
    }
}
