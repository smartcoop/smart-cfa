using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Smart.FA.Catalog.Core.Services;

namespace Smart.FA.Catalog.Web.Pages.Admin;

[Authorize(Policy = Policies.List.AtLeastOneValidUserChartRevisionApproval)]
public class IndexModel : AdminPage
{
    [BindProperty] public IUserIdentity UserIdentity { get; }

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
