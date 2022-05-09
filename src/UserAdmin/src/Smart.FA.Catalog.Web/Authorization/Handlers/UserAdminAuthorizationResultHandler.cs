using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Smart.FA.Catalog.Web.Authorization.Policy.Requirements;

namespace Smart.FA.Catalog.Web.Authorization.Handlers;

public class UserAdminAuthorizationResultHandler : IAuthorizationMiddlewareResultHandler
{
    private readonly AuthorizationMiddlewareResultHandler _defaultHandler = new();

    public async Task HandleAsync(
        RequestDelegate next,
        HttpContext context,
        AuthorizationPolicy policy,
        PolicyAuthorizationResult authorizeResult)
    {
        if (authorizeResult.Forbidden)
        {
            // In the case of a failed requirement to have approved at least one valid user chart, the user will be redirected to the user chart approval page
            if (authorizeResult.AuthorizationFailure!.FailedRequirements.OfType<AtLeastOneValidUserChartRevisionApprovalRequirement>().Any())
            {
                context.Response.Redirect("/UserChart");
            }

            return;
        }

        // Fall back to the default implementation.
        await _defaultHandler.HandleAsync(next, context, policy, authorizeResult);
    }
}
