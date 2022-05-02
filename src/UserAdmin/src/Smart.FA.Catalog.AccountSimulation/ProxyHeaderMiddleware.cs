using System.Globalization;
using System.Security.Principal;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Primitives;
using Smart.FA.Catalog.AccountSimulation.Pages;

namespace Smart.FA.Catalog.AccountSimulation;

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
