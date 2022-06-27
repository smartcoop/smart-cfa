using Microsoft.AspNetCore.Authorization;
using Smart.FA.Catalog.Core.Services;

namespace Smart.FA.Catalog.Web.Authorization.Policy.Requirements;

/// <summary>
/// Requires that the user is a social member (at the opposite of a permanent member)
/// </summary>
public class MustBeShareholderRequirement : IAuthorizationRequirement
{
}

public class MustBeShareholderHandler : AuthorizationHandler<MustBeShareholderRequirement>
{
    private readonly IUserIdentity _userIdentity;

    public MustBeShareholderHandler(IUserIdentity userIdentity)
    {
        _userIdentity = userIdentity;
    }


    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MustBeShareholderRequirement requirement)
    {
        if (_userIdentity.IsShareholder)
        {
            context.Succeed(requirement);
        }
        return Task.CompletedTask;
    }
}
