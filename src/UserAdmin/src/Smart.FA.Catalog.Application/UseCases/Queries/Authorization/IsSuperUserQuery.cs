using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Smart.FA.Catalog.Core.Extensions;
using Smart.FA.Catalog.Infrastructure.Persistence;

namespace Smart.FA.Catalog.Application.UseCases.Queries.Authorization;

public class IsSuperUserQuery : IRequest<bool>
{
    public int TrainerId { get; }

    public IsSuperUserQuery(int trainerId)
    {
        TrainerId = trainerId;
    }
}

public class IsSuperUserQueryHandler : IRequestHandler<IsSuperUserQuery, bool>
{
    private readonly ILogger<IsSuperUserQueryHandler> _logger;
    private readonly CatalogContext _catalogContext;

    public IsSuperUserQueryHandler(ILogger<IsSuperUserQueryHandler> logger, CatalogContext catalogContext)
    {
        _logger = logger;
        _catalogContext = catalogContext;
    }

    public async Task<bool> Handle(IsSuperUserQuery query, CancellationToken cancellationToken)
    {
        try
        {
            return await _catalogContext
                .SuperUsers
                .AsNoTracking()
                .AnyAsync(superUser => superUser.TrainerId == query.TrainerId, cancellationToken);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, $"Error handling `{query.GetType().Namespace}`. Query: {query.ToJson()}");
            throw;
        }
    }
}
