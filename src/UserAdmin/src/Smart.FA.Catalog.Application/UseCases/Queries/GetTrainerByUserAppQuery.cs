using System.Linq.Expressions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Smart.FA.Catalog.Application.SeedWork;
using Smart.FA.Catalog.Core.Domain;
using Smart.FA.Catalog.Core.Domain.User.Dto;
using Smart.FA.Catalog.Core.Domain.User.Enumerations;
using Smart.FA.Catalog.Infrastructure.Persistence;

namespace Smart.FA.Catalog.Application.UseCases.Queries;

public class GetTrainerByUserAppQueryHandler : IRequestHandler<GetTrainerByUserAppRequest, GetTrainerByUserAppResponse>
{
    private readonly CatalogContext _context;

    public GetTrainerByUserAppQueryHandler(CatalogContext context)
    {
        _context = context;
    }

    public async Task<GetTrainerByUserAppResponse> Handle(GetTrainerByUserAppRequest query, CancellationToken cancellationToken)
    {
        var response = new GetTrainerByUserAppResponse();
        Expression<Func<Trainer, bool>> predicate = trainer => trainer.Identity.UserId == query.UserId && trainer.Identity.ApplicationTypeId == query.ApplicationType.Id;
        response.Trainer = await _context.Trainers.FirstOrDefaultAsync(predicate, cancellationToken);
        return response;
    }
}

public class GetTrainerByUserAppResponse : ResponseBase
{
    public Trainer? Trainer { get; set; }
    public UserDto User { get; set; } = null!;
}

public class GetTrainerByUserAppRequest : IRequest<GetTrainerByUserAppResponse>
{
    public ApplicationType ApplicationType { get; init; } = null!;

    public string UserId { get; init; } = null!;

    public GetTrainerByUserAppRequest()
    {
    }

    public GetTrainerByUserAppRequest(ApplicationType applicationType, string userId)
    {
        ApplicationType = applicationType;
        UserId = userId;
    }
}
