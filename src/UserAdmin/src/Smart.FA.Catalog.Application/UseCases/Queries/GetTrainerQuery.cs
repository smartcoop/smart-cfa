using MediatR;
using Microsoft.EntityFrameworkCore;
using Smart.FA.Catalog.Application.SeedWork;
using Smart.FA.Catalog.Core.Domain;
using Smart.FA.Catalog.Infrastructure.Persistence;

namespace Smart.FA.Catalog.Application.UseCases.Queries;

public class GetTrainerQuery: IRequestHandler<GetTrainerRequest, GetTrainerResponse>
{
    private readonly CatalogContext _catalogContext;

    public GetTrainerQuery(CatalogContext catalogContext)
    {
        _catalogContext = catalogContext;
    }
    public async Task<GetTrainerResponse> Handle(GetTrainerRequest request, CancellationToken cancellationToken)
    {
        GetTrainerResponse response = new();
        response.Trainer = await _catalogContext.Trainers.FirstAsync(  trainer => trainer.Id == request.TrainerId, cancellationToken);
        response.SetSuccess();

        return response;
    }
}

public class GetTrainerRequest: IRequest<GetTrainerResponse>
{
    public int TrainerId { get; set; }
}

public class GetTrainerResponse: ResponseBase
{
    public Trainer Trainer { get; set; } = null!;
}
