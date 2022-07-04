using MediatR;
using Smart.FA.Catalog.Application.SeedWork;
using Smart.FA.Catalog.Core.Domain.Authorization;
using Smart.FA.Catalog.Infrastructure.Persistence;

namespace Smart.FA.Catalog.Application.UseCases.Commands;

public class BlackListTrainerCommand : IRequestHandler<BlackListTrainerRequest, BlackListTrainerResponse>
{
    private readonly CatalogContext _catalogContext;

    public BlackListTrainerCommand(CatalogContext catalogContext)
    {
        _catalogContext = catalogContext;
    }

    public async Task<BlackListTrainerResponse> Handle(BlackListTrainerRequest command, CancellationToken cancellationToken)
    {
        BlackListTrainerResponse response = new();
        var blackListedUser = new BlackListedTrainer(command.TrainerId);
        _catalogContext.BlackListedTrainer.Add(blackListedUser);
        await _catalogContext.SaveChangesAsync(cancellationToken);
        response.SetSuccess();

        return response;
    }
}

public class BlackListTrainerRequest : IRequest<BlackListTrainerResponse>
{
    public int TrainerId { get; set; }
}

public class BlackListTrainerResponse : ResponseBase
{
}
