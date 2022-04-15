using MediatR;
using Microsoft.Extensions.Logging;
using Smart.FA.Catalog.Application.SeedWork;
using Smart.FA.Catalog.Core.Domain;
using Smart.FA.Catalog.Core.LogEvents;
using Smart.FA.Catalog.Infrastructure.Persistence;

namespace Smart.FA.Catalog.Application.UseCases.Commands;

public class UploadUserChartCommand : IRequestHandler<UploadUserChartRequest, UploadUserChartResponse>
{
    private readonly ILogger<UploadUserChartCommand> _logger;
    private readonly CatalogContext _context;

    public UploadUserChartCommand(ILogger<UploadUserChartCommand> logger, CatalogContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<UploadUserChartResponse> Handle(UploadUserChartRequest request, CancellationToken cancellationToken)
    {
        UploadUserChartResponse response = new();
        UserChart userChart = new(request.Title, request.Version, request.ValidityDate, request.ExpirationDate);
        _context.UserCharts.Add(userChart);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(LogEventIds.UserChartCreated, "A new user chart with title {Title} version {Version} has been uploaded", request.Title, request.Version);

        response.UserChart = userChart;
        response.SetSuccess();

        return response;
    }
}

public class UploadUserChartRequest : IRequest<UploadUserChartResponse>
{
    public string Title { get; set; } = null!;
    public string Version { get; set; } = null!;
    public DateTime? ValidityDate { get; set; }
    public DateTime? ExpirationDate { get; set; }
}

public class UploadUserChartResponse : ResponseBase
{
    public UserChart UserChart { get; set; }
}
