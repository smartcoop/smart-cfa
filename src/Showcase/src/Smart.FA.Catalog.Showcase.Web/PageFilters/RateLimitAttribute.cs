using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;

namespace Smart.FA.Catalog.Showcase.Web.PageFilters;

/// <summary>
/// Adds to the ModelState an error if an IP address reaches the rate limit on HTTP Post for a given time frame.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class RateLimitAttribute : Attribute, IPageFilter
{
    private IMemoryCache _memoryCache = null!;
    private readonly TimeSpan _timeFrame;

    public int MaximumRequests { get; init; }

    public int TimeFrameInSeconds { get; init; }

    public RateLimitAttribute(int maximumRequests, int timeFrameInSeconds)
    {
        if (maximumRequests < 0)
        {
            throw new ArgumentException("The maximum requests per time frame must be a positive interger");
        }

        if (timeFrameInSeconds < 0)
        {
            throw new ArgumentException("Rate limit's time frame in seconds must be a positive integer");
        }

        MaximumRequests = maximumRequests;
        TimeFrameInSeconds = timeFrameInSeconds;
        _timeFrame = TimeSpan.FromSeconds(TimeFrameInSeconds);
    }

    public void OnPageHandlerSelected(PageHandlerSelectedContext context)
    {
    }

    public void OnPageHandlerExecuting(PageHandlerExecutingContext context)
    {
        // If the ModelState is invalid there is no point to go any further.
        // We only restrict rate limit on HTTP Post.
        // We need to use StringComparison's OrdinalIgnoreCase because `context.HandlerMethod?.HttpMethod` can be equal to "Post" but HttpMethod.Post.Method equals "POST".
        if (context.ModelState.IsValid && context.HandlerMethod?.HttpMethod.Equals(HttpMethod.Post.Method, StringComparison.OrdinalIgnoreCase) is true)
        {
            _memoryCache = context.HttpContext.RequestServices.GetRequiredService<IMemoryCache>();

            var key = GenerateUniqueKey(context);

            var requests = _memoryCache.Get<LinkedList<DateTime>>(key) ?? new LinkedList<DateTime>();

            if (!CanProcess(requests))
            {
                context.ModelState.AddModelError(ModelStateErrorKeys.RateLimit, ShowcaseResources.YouReachedRateLimit);
                return;
            }

            UpdateTimeRate(key, requests);
        }
    }

    private string GenerateUniqueKey(PageHandlerExecutingContext context)
    {
        var httpContext = context.HttpContext;
        var remoteIp = httpContext.Connection.RemoteIpAddress;
        var page = (context.HandlerInstance as PageModel)!.PageContext.ActionDescriptor.DisplayName;
        var methodName = context.HandlerMethod!.MethodInfo.Name;
        var key = $"{remoteIp}-{page}-{methodName}";
        return key;
    }

    private void UpdateTimeRate(string key, LinkedList<DateTime> requests)
    {
        // Purges requests that older than the time frame allowed.
        while (requests.Count > 0 && DateTime.UtcNow - requests.First!.Value > _timeFrame)
        {
            requests.RemoveFirst();
        }

        requests.AddLast(DateTime.UtcNow);

        _memoryCache.Set(key, requests, _timeFrame);
    }

    private bool CanProcess(LinkedList<DateTime> requests)
    {
        if (requests.Count >= MaximumRequests && DateTime.UtcNow - requests.Last!.Value < _timeFrame)
        {
            return false;
        }

        return true;
    }

    public void OnPageHandlerExecuted(PageHandlerExecutedContext context)
    {
    }
}
