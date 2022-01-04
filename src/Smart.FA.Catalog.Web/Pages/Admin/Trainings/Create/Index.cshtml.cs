using Application.UseCases.Commands;
using Core.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Pages.Admin.Trainings.Create;

public class CreateModel : AdminPage
{
    [BindProperty] public CreateTrainingViewModel CreateTrainingViewModel { get; set; } = null!;
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

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return RedirectToPage();
        }

        var response = await Mediator.Send(CreateTrainingViewModel.MapToRequest(new Trainer("null", "null", "null", "FR"){Id = 1}));
        return RedirectToPage("/Admin/trainings/List");
    }

    protected override SideMenuItem GetSideMenuItem()
    {
        return SideMenuItem.MyTrainings;
    }

}
