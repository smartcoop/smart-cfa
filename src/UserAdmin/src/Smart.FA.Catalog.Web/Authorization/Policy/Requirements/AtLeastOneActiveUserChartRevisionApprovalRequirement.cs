using MediatR;
using Microsoft.AspNetCore.Authorization;
using Smart.FA.Catalog.Application.UseCases.Queries;
using Smart.FA.Catalog.Core.Services;

namespace Smart.FA.Catalog.Web.Authorization.Policy.Requirements;

/// <summary>
/// Requires that the user have already accepted at least one valid user chart
/// </summary>
public class AtLeastOneActiveUserChartRevisionApprovalRequirement : IAuthorizationRequirement
{
}

public class UserChartRevisionApprovalHandler : AuthorizationHandler<AtLeastOneActiveUserChartRevisionApprovalRequirement>
{
    private readonly IMediator _mediator;
    private readonly IUserIdentity _userIdentity;

    public UserChartRevisionApprovalHandler(IMediator mediator, IUserIdentity userIdentity)
    {
        _mediator = mediator;
        _userIdentity = userIdentity;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AtLeastOneActiveUserChartRevisionApprovalRequirement requirement)
    {
        if (_userIdentity.IsSuperUser || await _mediator.Send(new HasAcceptedOneActiveUserChartRevisionQuery(_userIdentity.Id)))
        {
            context.Succeed(requirement);
        }
    }
}
