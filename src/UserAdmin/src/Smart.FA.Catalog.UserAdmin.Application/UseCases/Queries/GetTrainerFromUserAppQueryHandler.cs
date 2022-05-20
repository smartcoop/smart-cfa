using System.Linq.Expressions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Smart.FA.Catalog.UserAdmin.Application.SeedWork;
using Smart.FA.Catalog.UserAdmin.Domain.Domain;
using Smart.FA.Catalog.UserAdmin.Domain.Domain.User.Dto;
using Smart.FA.Catalog.UserAdmin.Domain.Domain.User.Enumerations;
using Smart.FA.Catalog.UserAdmin.Infrastructure.Persistence;

namespace Smart.FA.Catalog.UserAdmin.Application.UseCases.Queries;

public class
    GetTrainerFromUserAppQueryHandler : IRequestHandler<GetTrainerFromUserAppRequest, GetTrainerFromUserAppResponse>
{
    private readonly CatalogContext _context;

    public GetTrainerFromUserAppQueryHandler(CatalogContext context)
    {
        _context = context;
    }

    public async Task<GetTrainerFromUserAppResponse> Handle(GetTrainerFromUserAppRequest query, CancellationToken cancellationToken)
    {
        var response = new GetTrainerFromUserAppResponse();
        Expression<Func<Trainer, bool>> predicate = trainer => trainer.Identity.UserId == query.UserId && trainer.Identity.ApplicationTypeId == query.ApplicationType.Id;
        response.Trainer = await _context.Trainers.FirstOrDefaultAsync(predicate, cancellationToken);
        return response;
    }
}

public class GetTrainerFromUserAppResponse : ResponseBase
{
    public Trainer? Trainer { get; set; }
    public UserDto User { get; set; } = null!;
}

public class GetTrainerFromUserAppRequest : IRequest<GetTrainerFromUserAppResponse>
{
    public ApplicationType ApplicationType { get; init; } = null!;

    public string UserId { get; init; } = null!;

    public GetTrainerFromUserAppRequest()
    {
    }

    public GetTrainerFromUserAppRequest(ApplicationType applicationType, string userId)
    {
        ApplicationType = applicationType;
        UserId = userId;
    }
}
