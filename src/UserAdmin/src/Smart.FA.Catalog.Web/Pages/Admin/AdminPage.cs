using MediatR;

namespace Smart.FA.Catalog.Web.Pages.Admin;

public abstract class AdminPage : PageModelBase
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

