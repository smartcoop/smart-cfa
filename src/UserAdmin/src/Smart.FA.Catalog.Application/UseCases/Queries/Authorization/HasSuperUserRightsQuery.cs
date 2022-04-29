using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Smart.FA.Catalog.Core.Extensions;
using Smart.FA.Catalog.Infrastructure.Persistence;

namespace Smart.FA.Catalog.Application.UseCases.Queries.Authorization;

public class HasSuperUserRightsQuery : IRequest<bool>
{
    public string UserId { get; }

    public HasSuperUserRightsQuery(string userId)
    {
        UserId = userId;
    }
}

public class HasSuperUserRightsQueryHandler : IRequestHandler<HasSuperUserRightsQuery, bool>
{
    private readonly ILogger<HasSuperUserRightsQueryHandler> _logger;
    private readonly CatalogContext _catalogContext;

    public HasSuperUserRightsQueryHandler(ILogger<HasSuperUserRightsQueryHandler> logger, CatalogContext catalogContext)
    {
        _logger = logger;
        _catalogContext = catalogContext;
    }

    public async Task<bool> Handle(HasSuperUserRightsQuery query, CancellationToken cancellationToken)
    {
        try
        {
            return await _catalogContext
                .SuperUsers
                .AsNoTracking()
                .AnyAsync(superUser => superUser.UserId == query.UserId, cancellationToken: cancellationToken);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, $"Error handling `{query.GetType().Namespace}`. Query: {query.ToJson()}");
            throw;
        }
    }
}
