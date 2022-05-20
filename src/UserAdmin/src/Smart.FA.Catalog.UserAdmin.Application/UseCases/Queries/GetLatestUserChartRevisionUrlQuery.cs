using MediatR;
using Microsoft.Extensions.Logging;
using Smart.FA.Catalog.UserAdmin.Application.SeedWork;
using Smart.FA.Catalog.UserAdmin.Domain.Exceptions;
using Smart.FA.Catalog.UserAdmin.Infrastructure.Extensions;
using Smart.FA.Catalog.UserAdmin.Infrastructure.Helpers;
using Smart.FA.Catalog.UserAdmin.Infrastructure.Persistence;

namespace Smart.FA.Catalog.UserAdmin.Application.UseCases.Queries;

public class GetLatestUserChartRevisionUrlQuery : IRequestHandler<GetLatestUserChartRevisionUrlRequest, GetLatestUserChartRevisionUrlResponse>
{
    private readonly ILogger<GetLatestUserChartRevisionUrlQuery> _logger;
    private readonly CatalogContext _catalogContext;
    private readonly IMinIoLinkGenerator _minIoLinkGenerator;

    public GetLatestUserChartRevisionUrlQuery(ILogger<GetLatestUserChartRevisionUrlQuery> logger, CatalogContext catalogContext, IMinIoLinkGenerator minIoLinkGenerator)
    {
        _logger = logger;
        _catalogContext = catalogContext;
        _minIoLinkGenerator = minIoLinkGenerator;
    }

    public async Task<GetLatestUserChartRevisionUrlResponse> Handle(GetLatestUserChartRevisionUrlRequest request, CancellationToken cancellationToken)
    {
        GetLatestUserChartRevisionUrlResponse response = new();
        var userChart = await _catalogContext.UserChartRevisions.GetLatestCreatedOrDefaultAsync(cancellationToken);

        if (userChart is null)
        {
            throw new UserChartRevisionException(Errors.UserChartRevision.DontExist);
        }

        response.LatestUserChartRevisionUrl = _minIoLinkGenerator.GetAbsoluteUserChartUrl(userChart.Id);
        response.SetSuccess();
        return response;
    }
}

public class GetLatestUserChartRevisionUrlRequest : IRequest<GetLatestUserChartRevisionUrlResponse>
{
}

public class GetLatestUserChartRevisionUrlResponse : ResponseBase
{
    public string LatestUserChartRevisionUrl { get; set; } = null!;
}
