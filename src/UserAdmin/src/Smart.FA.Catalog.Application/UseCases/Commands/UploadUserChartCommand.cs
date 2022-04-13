using MediatR;
using Microsoft.Extensions.Logging;
using Smart.FA.Catalog.Application.SeedWork;
using Smart.FA.Catalog.Core.Domain;
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

    public Task<UploadUserChartResponse> Handle(UploadUserChartRequest request, CancellationToken cancellationToken)
    {
        UploadUserChartResponse response = new();
        UserChart userChart = new(request.Title, request.Version, request.ValidityDate, request.ExpirationDate);
        _context.UserCharts.Add(userChart);
        response.UserChart = userChart;
        response.SetSuccess();

        return Task.FromResult(response);
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
