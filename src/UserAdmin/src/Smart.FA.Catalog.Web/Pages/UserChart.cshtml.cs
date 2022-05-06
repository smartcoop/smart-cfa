using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Smart.FA.Catalog.Application.UseCases.Commands;
using Smart.FA.Catalog.Application.UseCases.Queries;
using Smart.FA.Catalog.Core.Services;

namespace Smart.FA.Catalog.Web.Pages;

public class UserChartModel : PageModel
{
    private readonly IMediator _mediator;

    private readonly IUserIdentity _userIdentity;

    [BindProperty]
    public bool HasAcceptedUserChart { get; set; } = false;

    public string LastChartUrl { get; private set; } = null!;

    public UserChartModel(IMediator mediator, IUserIdentity userIdentity)
    {
        _mediator = mediator;
        _userIdentity = userIdentity;
    }

    public async Task<ActionResult> OnGetAsync()
    {
        if (await ShouldReturnToHomePageAsync())
        {
            return RedirectToPage("/Admin/Index");
        }

        LastChartUrl = await GetLastChartUrlAsync();
        return Page();
    }

    public async Task<ActionResult> OnPostAsync()
    {
        if (await ShouldReturnToHomePageAsync())
        {
            return RedirectToPage("/Admin/Index");
        }

        if (!HasAcceptedUserChart)
        {
            ModelState.AddModelError(string.Empty, CatalogResources.AdminHomePage_HasNotAcceptedUserChart);
            return Page();
        }

        await _mediator.Send(new SetTrainerHasAcceptedLatestUserChartRequest { TrainerId = _userIdentity.Id });
        return RedirectToPage("/Admin/Index");
    }

    private async Task<bool> ShouldReturnToHomePageAsync()
    {
        // There is no point to go any further if the user is either a Super User or if he has accepted already an active chart.
        if (_userIdentity.IsSuperUser || await _mediator.Send(new HasAcceptedOneActiveUserChartRevisionQuery(_userIdentity.Id)))
        {
            return true;
        }

        return false;
    }

    public async Task<string> GetLastChartUrlAsync()
    {
        var response = await _mediator.Send(new GetLatestUserChartRevisionUrlRequest());

        // If we ever find ourselves in a case where no user chart can't be retrieved from storage, we should display the base user chart in wwwroot (to avoid any legal conflict)
        // However it also means it should be updated regularly
        return response.LatestUserChartUrl?.ToString() ?? "/default_user_chart.pdf";
    }
}
