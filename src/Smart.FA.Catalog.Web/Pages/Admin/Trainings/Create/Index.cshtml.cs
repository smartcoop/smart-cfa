using Core.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Web.Identity;

namespace Web.Pages.Admin.Trainings.Create;

public class CreateModel : AdminPage
{
    [BindProperty] public CreateTrainingViewModel CreateTrainingViewModel { get; set; } = new();
    public List<string> ValidationErrors { get; set; } = new();

    public CreateModel(IMediator mediator) : base(mediator)
    {
    }

    private void Init()
    {
        SetSideMenuItem();
    }

    public void OnGet()
    {
        Init();
    }

    public async Task<IActionResult> OnPostSaveAsync(CreateTrainingViewModel model)
    {
        var user = (HttpContext.User.Identity as CustomIdentity)!;
        var request = CreateTrainingViewModel!.MapToRequest(user.Trainer.Id, user.Trainer.DefaultLanguage);
        var response =
            await Mediator.Send(request);

        return RedirectToPage("/Admin/Trainings/List/Index");
    }

    public async Task<IActionResult> OnPostValidateModelAsync()
    {
        var user = (HttpContext.User.Identity as CustomIdentity)!;
        var response =
            await Mediator.Send(
                CreateTrainingViewModel.MapDraftToRequest(user.Trainer.Id, Language.Create("FR").Value));
        ValidationErrors = response.ValidationErrors;
        return Page();
    }

    protected override SideMenuItem GetSideMenuItem()
    {
        return SideMenuItem.MyTrainings;
    }
}
