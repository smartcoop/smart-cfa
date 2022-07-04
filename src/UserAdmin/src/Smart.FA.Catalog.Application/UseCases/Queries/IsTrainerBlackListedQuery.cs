using MediatR;
using Microsoft.EntityFrameworkCore;
using Smart.FA.Catalog.Infrastructure.Persistence;

namespace Smart.FA.Catalog.Application.UseCases.Queries;

public class IsTrainerBlackListedQuery : IRequestHandler<IsTrainerBlackListedRequest, bool>
{
    private readonly CatalogContext _catalogContext;

    public IsTrainerBlackListedQuery(CatalogContext catalogContext)
    {
        _catalogContext = catalogContext;
    }

    public async Task<bool> Handle(IsTrainerBlackListedRequest request, CancellationToken cancellationToken)
    {
        var blackListedTrainer = await _catalogContext.BlackListedTrainer.FirstOrDefaultAsync(blacklistedTrainer => blacklistedTrainer.TrainerId == request.TrainerId, cancellationToken);
        return blackListedTrainer is not null;
    }
}

public class IsTrainerBlackListedRequest : IRequest<bool>
{
    public int TrainerId { get; set; }
}
