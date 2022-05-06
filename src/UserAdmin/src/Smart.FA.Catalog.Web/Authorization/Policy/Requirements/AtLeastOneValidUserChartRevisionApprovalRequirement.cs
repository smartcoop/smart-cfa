using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Smart.FA.Catalog.Core.Services;
using Smart.FA.Catalog.Infrastructure.Persistence;

namespace Smart.FA.Catalog.Web.Authorization.Policy.Requirements;

/// <summary>
/// Require that the user have already accepted at least one valid user chart
/// </summary>
public class AtLeastOneValidUserChartRevisionApprovalRequirement : IAuthorizationRequirement
{
}

public class UserChartRevisionApprovalHandler : AuthorizationHandler<AtLeastOneValidUserChartRevisionApprovalRequirement>
{
    private readonly CatalogContext       _catalogContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserIdentity        _userIdentity;
    private readonly IWebHostEnvironment  _webHostEnvironment;

    public UserChartRevisionApprovalHandler(CatalogContext catalogContext, IHttpContextAccessor httpContextAccessor, IUserIdentity userIdentity, IWebHostEnvironment webHostEnvironment)
    {
        _catalogContext      = catalogContext;
        _httpContextAccessor = httpContextAccessor;
        _userIdentity        = userIdentity;
        _webHostEnvironment  = webHostEnvironment;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AtLeastOneValidUserChartRevisionApprovalRequirement requirement)
    {
        // If we are in staging we don't need to check for any approval of user charts
        if (!_webHostEnvironment.IsStaging())
        {
            var currentDate = DateTime.UtcNow.Date;

            var hasTrainerValidUserChartApprovals = await _catalogContext.Trainers
                                                                         .Where(trainer => trainer.Id == _userIdentity.Id)
                                                                         .Where(trainer => trainer.Approvals.Any(approval =>
                                                                              currentDate >= approval.UserChartRevision.ValidFrom.Date &&
                                                                              (approval.UserChartRevision.ValidUntil == null || currentDate <= approval.UserChartRevision.ValidUntil!.Value.Date)))
                                                                         .AnyAsync();

            if (hasTrainerValidUserChartApprovals)
            {
                _httpContextAccessor.HttpContext!.Response.Cookies.Append("HasAcceptedUserChartRevision", "true", new CookieOptions { MaxAge = TimeSpan.FromMinutes(1) });
                context.Succeed(requirement);
            }

            return;
        }

        context.Succeed(requirement);
    }
}
