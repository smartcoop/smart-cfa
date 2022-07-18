using MediatR;
using Microsoft.EntityFrameworkCore;
using Smart.FA.Catalog.Application.SeedWork;
using Smart.FA.Catalog.Core.Domain;
using Smart.FA.Catalog.Infrastructure.Persistence;

namespace Smart.FA.Catalog.Application.UseCases.Queries;

public class GetTrainerByIdQuery : IRequestHandler<GetTrainerByIdRequest, GetTrainerByIdResponse>
{
    private readonly CatalogContext _catalogContext;

    public GetTrainerByIdQuery(CatalogContext catalogContext)
    {
        _catalogContext = catalogContext;
    }

    public async Task<GetTrainerByIdResponse> Handle(GetTrainerByIdRequest byIdRequest, CancellationToken cancellationToken)
    {
        GetTrainerByIdResponse byIdResponse = new();
        byIdResponse.Trainer = await _catalogContext.Trainers.SingleAsync(trainer => trainer.Id == byIdRequest.TrainerId, cancellationToken);
        byIdResponse.SetSuccess();

        return byIdResponse;
    }
}

public class GetTrainerByIdRequest : IRequest<GetTrainerByIdResponse>
{
    public int TrainerId { get; set; }
}

public class GetTrainerByIdResponse : ResponseBase
{
    public Trainer Trainer { get; set; } = null!;
}
