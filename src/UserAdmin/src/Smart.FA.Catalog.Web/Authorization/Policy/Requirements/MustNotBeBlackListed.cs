using MediatR;
using Microsoft.AspNetCore.Authorization;
using Smart.FA.Catalog.Application.UseCases.Queries;
using Smart.FA.Catalog.Core.Services;

namespace Smart.FA.Catalog.Web.Authorization.Policy.Requirements;

/// <summary>
/// Requires that the user is not blacklisted.
/// </summary>
public class MustNotBeBlackListedRequirement : IAuthorizationRequirement
{
}

public class MustNotBeBlackListedRequirementHandler : AuthorizationHandler<MustNotBeBlackListedRequirement>
{
    private readonly IMediator _mediator;
    private readonly IUserIdentity _userIdentity;

    public MustNotBeBlackListedRequirementHandler(IMediator mediator, IUserIdentity userIdentity)
    {
        _mediator = mediator;
        _userIdentity = userIdentity;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, MustNotBeBlackListedRequirement requirement)
    {
        if (!await _mediator.Send(new IsTrainerBlackListedRequest { TrainerId = _userIdentity.Id }))
        {
            context.Succeed(requirement);
        }
    }
}
