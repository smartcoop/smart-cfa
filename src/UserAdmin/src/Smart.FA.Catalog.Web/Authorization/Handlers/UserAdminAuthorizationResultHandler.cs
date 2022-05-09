using System.Collections;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization.Policy;
using Smart.FA.Catalog.Core.Services;
using Smart.FA.Catalog.Web.Authorization.Policy.Requirements;

namespace Smart.FA.Catalog.Web.Authorization.Handlers;

public class UserAdminAuthorizationResultHandler : IAuthorizationMiddlewareResultHandler
{
    private IUserIdentity _userIdentity;
    private readonly AuthorizationMiddlewareResultHandler _defaultHandler = new();

    public async Task HandleAsync(
        RequestDelegate next,
        HttpContext context,
        AuthorizationPolicy policy,
        PolicyAuthorizationResult authorizeResult)
    {
        _userIdentity = context.RequestServices.GetRequiredService<IUserIdentity>();

        if (!_userIdentity.IsSuperUser && authorizeResult.Forbidden)
        {
            var authorizationFailure = authorizeResult.AuthorizationFailure!;

            // In the case of a failed requirement to have approved at least one valid user chart, the user will be redirected to the user chart approval page
            if (authorizationFailure.FailedRequirements.AnyOfType<AtLeastOneActiveUserChartRevisionApprovalRequirement>())
            {
                context.Response.Redirect("/cfa/userChart");
            }

            if (authorizationFailure.FailedRequirements.Any(failedRequirement =>
                    failedRequirement is RolesAuthorizationRequirement rolesAuthorizationRequirement && rolesAuthorizationRequirement.AllowedRoles.Contains("SuperUser")))
            {
                context.Response.Redirect("/cfa/admin");
            }

            return;
        }

        // Fall back to the default implementation.
        await _defaultHandler.HandleAsync(next, context, policy, authorizeResult);
    }
}
