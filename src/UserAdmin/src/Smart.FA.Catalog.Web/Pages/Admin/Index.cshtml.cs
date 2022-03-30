using MediatR;
using Smart.FA.Catalog.Core.Services;

namespace Smart.FA.Catalog.Web.Pages.Admin;

public class IndexModel : AdminPage
{
    public IUserIdentity UserIdentity { get; }

    public void OnGet()
    {
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
}
