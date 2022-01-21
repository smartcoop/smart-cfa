using Api.Extensions;
using Api.Identity;
using Core.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Pages.Admin.Trainings.Create;

public class CreateModel : AdminPage
{
    [BindProperty] public CreateTrainingViewModel CreateTrainingViewModel { get; set; }
    public List<string> ValidationErrors { get; set; } = new();
    public CreateModel(IMediator mediator) : base(mediator)
    {
    }

    private async Task InitAsync()
    {
        SetSideMenuItem();
    }

    public async Task OnGet()
    {
        await InitAsync();
    }

    public async Task<IActionResult> OnPostSaveAsync()
    {
        var user = (HttpContext.User.Identity as CustomIdentity)!;
        var response =
            await Mediator.Send(
                CreateTrainingViewModel.MapToRequest(user.Trainer.Id, user.Trainer.DefaultLanguage));

        return RedirectToPage("/Admin/Trainings/List/Index");
    }

    public async Task<IActionResult>  OnPostValidateModelAsync()
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
