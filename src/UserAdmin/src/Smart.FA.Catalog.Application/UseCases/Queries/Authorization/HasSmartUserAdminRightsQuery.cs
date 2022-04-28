using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Smart.FA.Catalog.Core.Extensions;
using Smart.FA.Catalog.Infrastructure.Persistence;

namespace Smart.FA.Catalog.Application.UseCases.Queries.Authorization;

public class HasSmartUserAdminRightsQuery : IRequest<bool>
{
    public string UserId { get; }

    public HasSmartUserAdminRightsQuery(string userId)
    {
        UserId = userId;
    }
}

public class HasSmartUserAdminRightsQueryHandler : IRequestHandler<HasSmartUserAdminRightsQuery, bool>
{
    private readonly ILogger<HasSmartUserAdminRightsQueryHandler> _logger;
    private readonly CatalogContext _catalogContext;

    public HasSmartUserAdminRightsQueryHandler(ILogger<HasSmartUserAdminRightsQueryHandler> logger, CatalogContext catalogContext)
    {
        _logger = logger;
        _catalogContext = catalogContext;
    }

    public async Task<bool> Handle(HasSmartUserAdminRightsQuery query, CancellationToken cancellationToken)
    {
        try
        {
            return await _catalogContext
                .SuperAdmins
                .AsNoTracking()
                .AnyAsync(superAdmin => superAdmin.UserId == query.UserId, cancellationToken: cancellationToken);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, $"Error handling `{query.GetType().Namespace}`. Query: {query.ToJson()}");
            throw;
        }
    }
}
