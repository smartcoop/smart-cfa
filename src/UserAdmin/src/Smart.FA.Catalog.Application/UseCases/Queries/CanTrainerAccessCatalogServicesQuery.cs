using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Smart.FA.Catalog.Infrastructure.Persistence;

namespace Smart.FA.Catalog.Application.UseCases.Queries;

/// <summary>
/// Determines if a trainer can access the services of catalog.
/// </summary>
public class CanTrainerAccessCatalogServicesQuery : IRequest<bool>
{
    public CanTrainerAccessCatalogServicesQuery(int trainerId)
    {
        TrainerId = trainerId;
    }

    public int TrainerId { get; set; }
}

/// <summary>
/// Handles <see cref="CanTrainerAccessCatalogServicesQuery" />.
/// </summary>
public class CanTrainerAccessCatalogServicesQueryHandler : IRequestHandler<CanTrainerAccessCatalogServicesQuery, bool>
{
    private readonly CatalogContext _catalogContext;
    private readonly ILogger<CanTrainerAccessCatalogServicesQueryHandler> _logger;

    public CanTrainerAccessCatalogServicesQueryHandler(ILogger<CanTrainerAccessCatalogServicesQueryHandler> logger, CatalogContext catalogContext)
    {
        _logger = logger;
        _catalogContext = catalogContext;
    }

    public async Task<bool> Handle(CanTrainerAccessCatalogServicesQuery query, CancellationToken cancellationToken)
    {
        try
        {
            var currentDate = DateTime.UtcNow.Date;
            return await _catalogContext.Trainers
                .Where(trainer => trainer.Id == query.TrainerId)
                .Where(trainer => trainer.Approvals.Any(approval =>
                    currentDate >= approval.UserChartRevision.ValidFrom.Date &&
                    (approval.UserChartRevision.ValidUntil == null || currentDate <= approval.UserChartRevision.ValidUntil!.Value.Date)))
                .AnyAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "An error occurred while checking if trainer `{trainerId}` can access our services.", query.TrainerId);
            throw;
        }
    }
}
