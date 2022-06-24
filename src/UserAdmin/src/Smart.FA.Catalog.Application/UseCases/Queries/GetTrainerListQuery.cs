using MediatR;
using Microsoft.EntityFrameworkCore;
using Smart.FA.Catalog.Application.SeedWork;
using Smart.FA.Catalog.Core.Domain;
using Smart.FA.Catalog.Infrastructure.Persistence;

namespace Smart.FA.Catalog.Application.UseCases.Queries;

public class GetTrainerListQuery : IRequestHandler<GetTrainerListRequest, GetTrainerListResponse>
{
    private readonly CatalogContext _catalogContext;

    public GetTrainerListQuery(CatalogContext catalogContext)
    {
        _catalogContext = catalogContext;
    }

    public async Task<GetTrainerListResponse> Handle(GetTrainerListRequest request, CancellationToken cancellationToken)
    {
        GetTrainerListResponse response = new()
        {
            Trainers = await _catalogContext.Trainers
                .Include(trainer => trainer.SocialNetworks)
                .Include(trainer => trainer.Approvals)
                .Include(trainer => trainer.Assignments)
                .AsNoTracking().ToListAsync(cancellationToken)
        };
        response.SetSuccess();
        return response;
    }
}

public class GetTrainerListRequest : IRequest<GetTrainerListResponse>
{
}

public class GetTrainerListResponse : ResponseBase
{
    public List<Trainer> Trainers { get; set; }
}
