using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Smart.FA.Catalog.Application.Extensions;
using Smart.FA.Catalog.Application.SeedWork;
using Smart.FA.Catalog.Core.Services;
using Smart.FA.Catalog.Infrastructure.Persistence;

namespace Smart.FA.Catalog.Application.UseCases.Queries;

public class GetLastUserChartUrlQuery : IRequestHandler<GetLastUserChartUrlRequest, GetLastUserChartUrlResponse>
{
    private readonly ILogger<GetLastUserChartUrlQuery> _logger;
    private readonly CatalogContext _catalogContext;
    private readonly IS3StorageService _storageService;

    public GetLastUserChartUrlQuery(ILogger<GetLastUserChartUrlQuery> logger, CatalogContext catalogContext, IS3StorageService storageService)
    {
        _logger = logger;
        _catalogContext = catalogContext;
        _storageService = storageService;
    }

    public async Task<GetLastUserChartUrlResponse> Handle(GetLastUserChartUrlRequest request, CancellationToken cancellationToken)
    {
        GetLastUserChartUrlResponse response = new();
        var userChart = await _catalogContext.UserCharts.OrderByDescending(userChart => userChart.CreatedAt).FirstAsync(cancellationToken);

        var userChartUrl = _storageService.GetPreSignedUrl(userChart.GenerateUserChartName(), DateTime.UtcNow.AddHours(1))!;

        response.LastUserChartUrl = ReplaceWithGlobalHost(userChartUrl);
        response.SetSuccess();
        return response;
    }

    /// <summary>
    /// A workaround for local dev' for a common issue with dockerized minio. The pre-signed url send back by a container minio server is the internal docker url
    /// which obviously can't be accessed outside of the docker context.
    /// </summary>
    /// <see cref="https://github.com/minio/minio-js/issues/514"/>
    /// <param name="baseUrl"></param>
    /// <returns></returns>
    private Uri ReplaceWithGlobalHost(Uri? baseUrl)
    {
        var uriBuilder = baseUrl is null ? null : new UriBuilder(baseUrl);

        _logger.LogInformation("Generated url for user chart pdf is {Url}", uriBuilder?.Uri.ToString() ?? string.Empty);

        //TODO: We will need to figure out another workaround for Dev' and Staging deployment with devops since minio is dockerized and port mapping on host machine are not allowed
        if (uriBuilder is not null && string.Equals(uriBuilder.Host, "host.docker.internal"))
        {
            uriBuilder.Scheme = "http";
            uriBuilder.Host = "localhost";
        }

        return uriBuilder!.Uri;
    }
}

public class GetLastUserChartUrlRequest : IRequest<GetLastUserChartUrlResponse>
{
}

public class GetLastUserChartUrlResponse : ResponseBase
{
    public Uri LastUserChartUrl { get; set; } = null!;
}
