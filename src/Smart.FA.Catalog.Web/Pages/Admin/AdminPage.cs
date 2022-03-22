using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Smart.FA.Catalog.Web.Pages.Admin;

public abstract class AdminPage : PageModel
{
    protected IMediator Mediator { get; }

    protected AdminPage(IMediator mediator) : base()
    {
        Mediator = mediator;
    }

    public void SetSideMenuItem()
    {
        ViewData[nameof(SideMenuItem)] = GetSideMenuItem();
    }

    protected abstract SideMenuItem GetSideMenuItem();
}

