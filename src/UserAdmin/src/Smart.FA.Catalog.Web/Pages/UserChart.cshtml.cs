using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Smart.FA.Catalog.Application.UseCases.Commands;
using Smart.FA.Catalog.Application.UseCases.Queries;
using Smart.FA.Catalog.Core.Services;

namespace Smart.FA.Catalog.Web.Pages;

public class UserChartModel : PageModel
{
    private IMediator Mediator { get; }

    public IUserIdentity UserIdentity { get; }

    [BindProperty] public bool HasAcceptedUserChart { get; set; } = false;

    public ActionResult OnGet()
    {
        return Page();
    }

    public UserChartModel(IMediator mediator, IUserIdentity userIdentity)
    {
        Mediator = mediator;
        UserIdentity = userIdentity;
    }

    public async Task<ActionResult> OnPostAsync()
    {
        if (HasAcceptedUserChart is false)
        {
            ModelState.AddModelError(string.Empty, CatalogResources.AdminHomePage_HasNotAcceptedUserChart);
            return Page();
        }

        await Mediator.Send(new SetTrainerHasAcceptedLatestUserChartRequest { TrainerId = UserIdentity.Id });
        return RedirectToPage("/Admin/Index");
    }

    public async Task<string> GetLastChartUrl()
    {
        var response = await Mediator.Send(new GetLatestUserChartRevisionUrlRequest());

        return response.LatestUserChartRevisionUrl;
    }
}
