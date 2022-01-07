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

        var response =
            await Mediator.Send(
                CreateTrainingViewModel.MapToRequest(new TrainerDto {Id = 1, DefaultLanguage = Language.Create("FR").Value}));

        return RedirectToPage("/Admin/Trainings/List/Index");
    }

    protected override SideMenuItem GetSideMenuItem()
    {
        return SideMenuItem.MyTrainings;
    }
}
