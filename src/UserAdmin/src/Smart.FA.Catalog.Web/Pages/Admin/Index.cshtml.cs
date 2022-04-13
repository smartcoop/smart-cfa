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

        await Mediator.Send(new SetTrainerHasAcceptedLastUserChartRequest {TrainerId = UserIdentity.Id});
        return RedirectToPage("/Admin/Trainings/List/Index");
    }

    public async Task<string> GetLastChartUrl()
    {
        var response = await Mediator.Send(new GetLastUserChartUrlRequest());

        return response.LastUserChartUrl.ToString();
    }
}
