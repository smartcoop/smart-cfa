using System.Security.Claims;
using System.Security.Principal;
using MediatR;
using Smart.FA.Catalog.Application.UseCases.Commands;
using Smart.FA.Catalog.Application.UseCases.Queries;
using Smart.FA.Catalog.Application.UseCases.Queries.Authorization;
using Smart.FA.Catalog.Core.Domain.Models;
using Smart.FA.Catalog.Core.Domain.User.Enumerations;
using Smart.FA.Catalog.Shared.Domain.Enumerations;

namespace Smart.FA.Catalog.Web.Extensions.Middlewares;

/// <summary>
/// The authentication is working with a system from Nginx (the proxy in front of our apps) called x-accel (https://www.nginx.com/resources/wiki/start/topics/examples/x-accel/).
/// The proxy header middleware parse incoming request from nginx to fetch a user and the name of the application the call originated.
/// It will then proceed to log the user as a trainer for the rest of the request lifetime.
/// </summary>
public class ProxyHeaderMiddleware
{
    private readonly RequestDelegate _next;
    private IMediator _mediator = null!;

    public ProxyHeaderMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IMediator mediator )
    {
        _mediator = mediator;

        //Default values are used to substitute to the Nginx authentication system
        //TODO: When the nginx authentication system is in place in every environment, remove default values
        var response = await GetTrainerBySmartUserIdAndApplicationTypeAsync(context);

        if (response.Trainer is null)
        {
            await CreateTrainerAsync(response);
        }

        await SetUserIdentityAsync(context, response);
        await _next(context);
    }

    private async Task<GetTrainerFromUserAppResponse> GetTrainerBySmartUserIdAndApplicationTypeAsync(HttpContext context)
    {
        var userId = (string.IsNullOrEmpty(context.Request.Headers["user_id"].ToString()) ? "1" : context.Request.Headers["user_id"].ToString())!;
        var appName = string.IsNullOrEmpty(context.Request.Headers["app_name"].ToString()) ? ApplicationType.Account.Name : context.Request.Headers["app_name"].ToString();
        var applicationType = Enumeration.FromDisplayName<ApplicationType>(appName);

        var response = await _mediator.Send(new GetTrainerFromUserAppRequest { UserId = userId, ApplicationType = applicationType });
        return response;
    }

    private async Task CreateTrainerAsync(GetTrainerFromUserAppResponse response)
    {
        var newTrainerResponse = await _mediator.Send(new CreateTrainerFromUserAppRequest { User = response.User });
        response.Trainer = newTrainerResponse.Trainer;
    }

    private async Task SetUserIdentityAsync(HttpContext context, GetTrainerFromUserAppResponse response)
    {
        var isAdmin  = await _mediator.Send(new HasSmartUserAdminRightsQuery(response.User.UserId));
        context.User = new GenericPrincipal(new CustomIdentity(response.Trainer!), roles: isAdmin ? new[] { "SuperAdmin" } : null);
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
