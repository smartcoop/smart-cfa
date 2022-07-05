using MediatR;
using Microsoft.AspNetCore.Authorization;
using Smart.FA.Catalog.Application.UseCases.Queries;
using Smart.FA.Catalog.Core.Services;

namespace Smart.FA.Catalog.Web.Authorization.Policy.Requirements;

public class MustBeSuperUserOrTrainingCreator : IAuthorizationRequirement
{
}

public class MustBeSuperUserOrTrainingCreatorHandler : AuthorizationHandler<MustBeSuperUserOrTrainingCreator>
{
    private readonly IUserIdentity _userIdentity;
    private readonly IMediator _mediator;

    public MustBeSuperUserOrTrainingCreatorHandler(IUserIdentity userIdentity, IMediator mediator)
    {
        _userIdentity = userIdentity;
        _mediator = mediator;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, MustBeSuperUserOrTrainingCreator requirement)
    {
        // Super admin can access whatever they want.
        if (_userIdentity.IsSuperUser)
        {
            context.Succeed(requirement);
        }

        if (context.Resource is DefaultHttpContext httpContext)
        {
            await ValidateSuccessIfCreatorOfTrainingAsync(context, requirement, httpContext);
        }
    }

    private async Task ValidateSuccessIfCreatorOfTrainingAsync(AuthorizationHandlerContext context,
        MustBeSuperUserOrTrainingCreator requirement,
        DefaultHttpContext httpContext)
    {
        var success = int.TryParse(httpContext.GetRouteValue("id") as string, out var trainingId);
        if (success)
        {
            var trainingFromIdResponse = await _mediator.Send(new GetTrainingByIdRequest() { TrainingId = trainingId });

            if (trainingFromIdResponse.Training?.CreatedBy == _userIdentity.Id)
            {
                context.Succeed(requirement);
            }
        }
    }
}
