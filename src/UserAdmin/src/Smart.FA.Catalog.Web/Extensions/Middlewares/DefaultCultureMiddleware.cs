using System.Globalization;

namespace Smart.FA.Catalog.Web.Extensions.Middlewares;

public class DefaultCultureMiddleware
{
    private readonly RequestDelegate _next;

    public DefaultCultureMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var cultureInfo = new CultureInfo("fr-BE") { NumberFormat = { CurrencySymbol = "€" } };

        CultureInfo.CurrentCulture = cultureInfo;
        CultureInfo.CurrentUICulture = cultureInfo;

        await _next(context);
    }
}

public static class DefaultCultureMiddlewareExtensions
{
    public static IApplicationBuilder UseDefaultCulture(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<DefaultCultureMiddleware>();
    }
}
