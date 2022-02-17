using MediatR;
using Microsoft.AspNetCore.Mvc;
using Web.Identity;

namespace Web.Pages.Admin.Trainings.Create;

public class CreateModel : AdminPage
{
    private readonly ILogger<CreateModel> _logger;
    [BindProperty] public CreateTrainingViewModel CreateTrainingViewModel { get; set; } = new();
    public List<string> ValidationErrors { get; set; } = new();

    public CreateModel(IMediator mediator, ILogger<CreateModel> logger) : base(mediator)
    {
        _logger = logger;
    }

    private void Init()
    {
        SetSideMenuItem();
    }

    public void OnGet()
    {
        Init();
    }

    public async Task<IActionResult> OnPostSaveAsync()
    {
        var user = (HttpContext.User.Identity as CustomIdentity)!;
        if (!ModelState.IsValid)
        {
            return Page();
        }
        var request = CreateTrainingViewModel.MapToRequest(user.Trainer.Id, user.Trainer.DefaultLanguage);
        var response = await Mediator.Send(request);
        return RedirectToPage("/Admin/Trainings/List/Index");
    }

    protected override SideMenuItem GetSideMenuItem()
    {
        return SideMenuItem.MyTrainings;
    }
}
