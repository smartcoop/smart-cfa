using MediatR;
using Microsoft.EntityFrameworkCore;
using Smart.FA.Catalog.Application.SeedWork;
using Smart.FA.Catalog.Infrastructure.Persistence;

namespace Smart.FA.Catalog.Application.UseCases.Commands;

public class WhiteListTrainerCommand : IRequestHandler<WhiteListTrainerRequest, WhiteListTrainerResponse>
{
    private readonly CatalogContext _catalogContext;

    public WhiteListTrainerCommand(CatalogContext catalogContext)
    {
        _catalogContext = catalogContext;
    }

    public async Task<WhiteListTrainerResponse> Handle(WhiteListTrainerRequest request, CancellationToken cancellationToken)
    {
        WhiteListTrainerResponse response = new();
        var blackListedUser = await _catalogContext.BlackListedTrainer
            .FirstOrDefaultAsync(trainer => trainer.TrainerId == request.TrainerId, cancellationToken);
        if (blackListedUser is not null)
        {
            _catalogContext.BlackListedTrainer.Remove(blackListedUser);
            await _catalogContext.SaveChangesAsync(cancellationToken);
        }

        response.SetSuccess();
        return response;
    }
}

public class WhiteListTrainerRequest : IRequest<WhiteListTrainerResponse>
{
    public int TrainerId { get; set; }
}

public class WhiteListTrainerResponse : ResponseBase
{
}
