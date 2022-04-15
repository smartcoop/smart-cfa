using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Smart.FA.Catalog.Application.UseCases.Commands;
using Smart.FA.Catalog.Application.UseCases.Queries;
using Smart.FA.Catalog.Core.Services;

namespace Smart.FA.Catalog.Web.Pages.Admin;

public class IndexModel : AdminPage
{
    public IUserIdentity UserIdentity { get; }
    [BindProperty] public bool HasAcceptedUserChart { get; set; } = false;

    public async Task<IActionResult> OnGetAsync()
    {
        if (HttpContext.Request.Cookies.TryGetValue("AtLeastOneValidUserChartApproved", out _))
        {
            return RedirectToPage("/Admin/Trainings/List/Index");
        }

        return Page();
    }

    public IndexModel(IMediator mediator, IUserIdentity userIdentity) : base(mediator)
    {
        UserIdentity = userIdentity;
    }

    protected override SideMenuItem GetSideMenuItem()
    {
        // Doesn't matter we return MyTrainings or whatever, we don't see it anyway.
        return SideMenuItem.MyTrainings;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (HasAcceptedUserChart is false)
        {
            ModelState.AddModelError(string.Empty, CatalogResources.AdminHomePage_HasNotAcceptedUserChart);
        }

        await Mediator.Send(new SetTrainerHasAcceptedLatestUserChartRequest {TrainerId = UserIdentity.Id});
        return RedirectToPage("/Admin/Trainings/List/Index");
    }

    public async Task<string> GetLastChartUrl()
    {
        var response = await Mediator.Send(new GetLatestUserChartUrlRequest());

        // If we ever find ourselves in a case where no user chart can be retrieved from storage, we should display the base user chart in wwwroot (to avoid any legal conflict)
        // However it also means it should be updated regularly
        return response.LastUserChartUrl?.ToString() ?? "/default_user_chart.pdf";
    }
}
