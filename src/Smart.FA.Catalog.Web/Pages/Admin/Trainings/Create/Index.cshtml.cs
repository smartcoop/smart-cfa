using Application.UseCases.Commands;
using Core.Domain;
using Core.Domain.Dto;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Pages.Admin.Trainings.Create;

public class CreateModel : AdminPage
{
    [BindProperty] public CreateTrainingViewModel CreateTrainingViewModel { get; set; } = new();

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

        var trainer = new TrainerDto(1, Name.Create("firstName", "lastName").Value, "", Language.Create("FR").Value );
        var response =
            await Mediator.Send(
                CreateTrainingViewModel.MapToRequest(trainer));

        return RedirectToPage("/Admin/Trainings/List/Index");
    }

    protected override SideMenuItem GetSideMenuItem()
    {
        return SideMenuItem.MyTrainings;
    }
}
