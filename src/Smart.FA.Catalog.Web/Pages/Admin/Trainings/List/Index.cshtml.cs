using Api.Options;
using Application.UseCases.Commands;
using Application.UseCases.Queries;
using Core.Domain;
using Core.Domain.Dto;
using Core.SeedWork;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Api.Pages.Admin.Trainings.List;

public class ListModel : AdminPage
{
    private readonly AdminOptions _adminOptions;
    public PagedList<TrainingDto>? Trainings { get; set; }
    public int NumberOfTrainingPerPage { get; set; }

    [BindProperty(SupportsGet = true)] public int CurrentPage { get; set; } = 1;

    public ListModel(IMediator mediator, IOptions<AdminOptions> adminOptions) : base(mediator)
    {
        _adminOptions = adminOptions.Value ?? throw new ArgumentNullException(nameof(adminOptions));
        NumberOfTrainingPerPage = 2;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        SetSideMenuItem();
        var response = await Mediator.Send(new GetPagedTrainingsFromTrainerRequest
        {
            TrainerId = 1,
            Language = Language.Create("FR").Value,
            PageItem = new PageItem(CurrentPage, NumberOfTrainingPerPage)
        });
        Trainings = response.Trainings;
        return Page();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var response = await Mediator.Send(new DeleteTrainingRequest {TrainingId = id});
        return RedirectToPage(new {CurrentPage = 1});
    }
    protected override SideMenuItem GetSideMenuItem() => SideMenuItem.MyTrainings;
}
