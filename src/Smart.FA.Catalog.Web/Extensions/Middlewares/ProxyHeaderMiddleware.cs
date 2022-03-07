using System.Globalization;
using System.Security.Principal;
using Application.UseCases.Commands;
using Application.UseCases.Queries;
using Core.Domain.Enumerations;
using Core.Domain.Models;
using Core.SeedWork;
using MediatR;

namespace Web.Extensions.Middlewares;

/// <summary>
/// The authentication is working with a system from Nginx (the proxy in front of our apps) called x-accel (https://www.nginx.com/resources/wiki/start/topics/examples/x-accel/).
/// The proxy header middleware parse incoming request from nginx to fetch a user and the name of the application the call originated.
/// It will then proceed to log the user as a trainer for the rest of the request lifetime.
/// </summary>
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
        var appName =  string.IsNullOrEmpty(context.Request.Headers["app_name"].ToString()) ? ApplicationType.Account.Name : context.Request.Headers["app_name"].ToString();
        var applicationType = Enumeration.FromDisplayName<ApplicationType>(appName);

        var userResponse = await mediator.Send(new GetUserAppFromIdRequest{UserId = userId, ApplicationType = applicationType});
        var trainerResponse = await mediator.Send(new GetTrainerFromUserAppRequest {User = userResponse.User});

        if (trainerResponse.Trainer is null)
        {
            var newTrainerResponse = await mediator.Send(new CreateTrainerFromUserAppRequest{User = userResponse.User});
            trainerResponse.Trainer = newTrainerResponse.Trainer;
        }

        CultureInfo.CurrentUICulture = new CultureInfo(trainerResponse.Trainer!.DefaultLanguage.Value);
        context.User = new GenericPrincipal(new CustomIdentity(trainerResponse.Trainer!), null );
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
