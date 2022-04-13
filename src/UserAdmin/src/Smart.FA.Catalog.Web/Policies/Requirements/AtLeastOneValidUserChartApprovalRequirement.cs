using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Smart.FA.Catalog.Core.Domain.Models;
using Smart.FA.Catalog.Infrastructure.Persistence;

namespace Smart.FA.Catalog.Web.Policies.Requirements;

/// <summary>
/// Require that the user have already accepted at least one valid user chart
/// </summary>
public class AtLeastOneValidUserChartApprovalRequirement : IAuthorizationRequirement { }

public class UserChartApprovalHandler : AuthorizationHandler<AtLeastOneValidUserChartApprovalRequirement>
{
    private readonly CatalogContext _catalogContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserChartApprovalHandler(CatalogContext catalogContext, IHttpContextAccessor httpContextAccessor)
    {
        _catalogContext = catalogContext;
        _httpContextAccessor = httpContextAccessor;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AtLeastOneValidUserChartApprovalRequirement requirement)
    {
        var currentDate = DateTime.UtcNow;
        var identity = (CustomIdentity) context.User.Identity!;
        var validUserChartsApprovals = _catalogContext.Trainers
            .FirstOrDefault(trainer => trainer.Id == identity.Id)?
            .Approvals
            .Where(trainerApproval => (trainerApproval.UserChart.ValidityDate == null || trainerApproval.UserChart.ValidityDate!.Value.Ticks >= currentDate.Ticks) &&
                                      (trainerApproval.UserChart.ExpirationDate == null || trainerApproval.UserChart.ExpirationDate!.Value.Ticks <= currentDate.Ticks));
        if (validUserChartsApprovals is not null && validUserChartsApprovals.Any())
        {
            _httpContextAccessor.HttpContext!.Response.Cookies.Append("HasAcceptedUserChart", "true", new CookieOptions{MaxAge = TimeSpan.FromMinutes(1)});
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
