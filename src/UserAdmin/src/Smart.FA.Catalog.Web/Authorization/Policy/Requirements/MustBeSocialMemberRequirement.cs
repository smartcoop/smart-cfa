using Microsoft.AspNetCore.Authorization;
using Smart.FA.Catalog.Core.Services;

namespace Smart.FA.Catalog.Web.Authorization.Policy.Requirements;

/// <summary>
/// Requires that the user is a social member (at the opposite of a permanent member)
/// </summary>
public class MustBeSocialMemberRequirement : IAuthorizationRequirement
{
}

public class MustBeSocialMemberHandler : AuthorizationHandler<MustBeSocialMemberRequirement>
{
    private readonly IUserIdentity _userIdentity;

    public MustBeSocialMemberHandler(IUserIdentity userIdentity)
    {
        _userIdentity = userIdentity;
    }


    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MustBeSocialMemberRequirement requirement)
    {
        if (_userIdentity.IsSocialMember)
        {
            context.Succeed(requirement);
        }
        return Task.CompletedTask;
    }
}
