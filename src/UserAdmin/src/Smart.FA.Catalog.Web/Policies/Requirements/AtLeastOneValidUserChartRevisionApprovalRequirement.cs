using MediatR;
using Microsoft.AspNetCore.Authorization;
using Smart.FA.Catalog.Application.UseCases.Queries;
using Smart.FA.Catalog.Core.Services;

namespace Smart.FA.Catalog.Web.Policies.Requirements;

/// <summary>
/// Require that the user have already accepted at least one valid user chart
/// </summary>
public class AtLeastOneValidUserChartRevisionApprovalRequirement : IAuthorizationRequirement
{
}

public class UserChartRevisionApprovalHandler : AuthorizationHandler<AtLeastOneValidUserChartRevisionApprovalRequirement>
{
    private readonly IMediator _mediator;
    private readonly IUserIdentity _userIdentity;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public UserChartRevisionApprovalHandler(IMediator mediator, IUserIdentity userIdentity, IWebHostEnvironment webHostEnvironment)
    {
        _mediator = mediator;
        _userIdentity = userIdentity;
        _webHostEnvironment = webHostEnvironment;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AtLeastOneValidUserChartRevisionApprovalRequirement requirement)
    {
        // No need for that if the connected user is an super admin.
        if (_userIdentity.IsSuperUser)
        {
            context.Succeed(requirement);
            return;
        }

        // If we are in staging we don't need to check for any approval of user charts
        if (_webHostEnvironment.IsStaging() || await _mediator.Send(new CanTrainerAccessCatalogServicesQuery(_userIdentity.Id)))
        {
            context.Succeed(requirement);
        }
    }
}
