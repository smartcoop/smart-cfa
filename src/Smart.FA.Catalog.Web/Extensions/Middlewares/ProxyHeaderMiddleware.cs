using System.Globalization;
using System.Security.Principal;
using Application.UseCases.Queries;
using Core.Domain.Enumerations;
using Core.SeedWork;
using MediatR;
using Web.Identity;

namespace Web.Extensions.Middlewares;

public class ProxyHeaderMiddleware
{
    private readonly RequestDelegate _next;

    public ProxyHeaderMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IMediator mediator )
    {

        var userId = (string.IsNullOrEmpty(context.Request.Headers["user_id"].ToString())  ? "1" : context.Request.Headers["user_id"].ToString())!;
        var appName =  string.IsNullOrEmpty(context.Request.Headers["app_name"].ToString()) ? ApplicationType.Default.Name : context.Request.Headers["app_name"].ToString();
        var applicationType = Enumeration.FromDisplayName<ApplicationType>(appName);

        var resp = await mediator.Send(new GetTrainerFromUserAppRequest {UserId = userId, ApplicationType = applicationType});

        CultureInfo.CurrentUICulture = new CultureInfo(resp.Trainer!.DefaultLanguage.Value);
        context.User = new GenericPrincipal(new CustomIdentity(resp.Trainer!), null );
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
