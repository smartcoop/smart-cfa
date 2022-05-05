using Microsoft.AspNetCore.Http.Extensions;

namespace Smart.FA.Catalog.AccountSimulator;

public class ProxyHeaderMiddleware
{
    private readonly RequestDelegate _next;

    public ProxyHeaderMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var request = context.Request.GetEncodedUrl();
        if (context.Request.Cookies.TryGetValue("user-id", out var userId))
        {
            var urlPathList = context.Request.Path.ToString().Split("cfa");
            var urlPath = urlPathList.Length > 1 ? urlPathList[1] : urlPathList[0];
            var cfaPath = string.IsNullOrEmpty(urlPath) ? "/" : urlPath;
            context.ProxyRedirect(cfaPath, userId);
        }

        await _next(context);
    }
}

public static class ProxyHeaderMiddlewareExtension
{
    public static IApplicationBuilder UseProxyHeaders(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ProxyHeaderMiddleware>();
    }
}
