using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Smart.FA.Catalog.Core.Domain.Models;
using Smart.FA.Catalog.Core.Services;
using Smart.FA.Catalog.Infrastructure.Persistence;

namespace Smart.FA.Catalog.Web.Policies.Requirements;

/// <summary>
/// Require that the user have already accepted at least one valid user chart
/// </summary>
public class AtLeastOneValidUserChartApprovalRequirement : IAuthorizationRequirement
{
}

public class UserChartApprovalHandler : AuthorizationHandler<AtLeastOneValidUserChartApprovalRequirement>
{
    private readonly CatalogContext _catalogContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserIdentity _userIdentity;

    public UserChartApprovalHandler(CatalogContext catalogContext, IHttpContextAccessor httpContextAccessor, IUserIdentity userIdentity)
    {
        _catalogContext = catalogContext;
        _httpContextAccessor = httpContextAccessor;
        _userIdentity = userIdentity;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AtLeastOneValidUserChartApprovalRequirement requirement)
    {
        var currentDate = DateTime.UtcNow;

        var hasTrainerValidUserChartApprovals = await _catalogContext.Trainers
            .Where(trainer => trainer.Id == _userIdentity.Id)
            .Where(trainer => trainer.Approvals.Any(approval =>
                currentDate >= approval.UserChartRevision.ValidFrom.Date &&
                (approval.UserChartRevision.ValidUntil == null || currentDate <= approval.UserChartRevision.ValidUntil!.Value.Date)))
            .AnyAsync();

        if (hasTrainerValidUserChartApprovals)
        {
            _httpContextAccessor.HttpContext!.Response.Cookies.Append("HasAcceptedUserChart", "true", new CookieOptions {MaxAge = TimeSpan.FromMinutes(1)});
            context.Succeed(requirement);
        }
    }
}
