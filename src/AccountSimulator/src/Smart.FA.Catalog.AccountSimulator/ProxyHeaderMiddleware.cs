using Microsoft.AspNetCore.Http.Extensions;

namespace Smart.FA.Catalog.AccountSimulator;

public class ProxyHeaderMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IAccountDataHeaderSerializer _accountDataHeaderSerializer;

    public ProxyHeaderMiddleware(RequestDelegate next, IAccountDataHeaderSerializer accountDataHeaderSerializer)
    {
        _next = next;
        _accountDataHeaderSerializer = accountDataHeaderSerializer;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var request = context.Request.GetEncodedUrl();
        if (context.Request.Cookies.TryGetValue("userid", out var userId))
        {
            var urlPathList = context.Request.Path.ToString().Split("cfa");
            var urlPath = urlPathList.Length > 1 ? urlPathList[1] : urlPathList[0];
            var cfaPath = string.IsNullOrEmpty(urlPath) ? "/" : urlPath;
            cfaPath += context.Request.QueryString.Value;
            var serializedAccountData = _accountDataHeaderSerializer.Serialize(_accountDataHeaderSerializer.GetByUserId(userId!));
            context.ProxyRedirect(cfaPath, userId, serializedAccountData);
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
