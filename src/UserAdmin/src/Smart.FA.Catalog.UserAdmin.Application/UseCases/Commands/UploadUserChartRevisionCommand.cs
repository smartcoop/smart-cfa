using MediatR;
using Microsoft.Extensions.Logging;
using Smart.FA.Catalog.UserAdmin.Application.SeedWork;
using Smart.FA.Catalog.UserAdmin.Domain.Domain;
using Smart.FA.Catalog.UserAdmin.Domain.LogEvents;
using Smart.FA.Catalog.UserAdmin.Infrastructure.Persistence;

namespace Smart.FA.Catalog.UserAdmin.Application.UseCases.Commands;

public class UploadUserChartRevisionCommand : IRequestHandler<UploadUserChartRevisionRequest, UploadUserChartRevisionResponse>
{
    private readonly ILogger<UploadUserChartRevisionCommand> _logger;
    private readonly CatalogContext _context;

    public UploadUserChartRevisionCommand(ILogger<UploadUserChartRevisionCommand> logger, CatalogContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<UploadUserChartRevisionResponse> Handle(UploadUserChartRevisionRequest revisionRequest, CancellationToken cancellationToken)
    {
        UploadUserChartRevisionResponse response = new();
        var userChartValidFrom = revisionRequest.ValidFrom ?? DateTime.UtcNow;
        UserChartRevision userChart = new(revisionRequest.Title, revisionRequest.Version, userChartValidFrom, revisionRequest.ValidUntil);
        _context.UserChartRevisions.Add(userChart);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(LogEventIds.UserChartRevisionCreated, "A new user chart with title {Title} version {Version} has been uploaded", revisionRequest.Title, revisionRequest.Version);

        response.UserChart = userChart;
        response.SetSuccess();

        return response;
    }
}

public class UploadUserChartRevisionRequest : IRequest<UploadUserChartRevisionResponse>
{
    public string Title { get; set; } = null!;
    public string Version { get; set; } = null!;
    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidUntil { get; set; }
}

public class UploadUserChartRevisionResponse : ResponseBase
{
    public UserChartRevision UserChart { get; set; } = null!;
}
