using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Smart.FA.Catalog.Application.Models.Options;
using Smart.FA.Catalog.Core.Services;
using Smart.FA.Catalog.Web.Identity;
using Smart.FA.Catalog.Web.Options;
using Smart.FA.Catalog.Web.Policies;
using Smart.FA.Catalog.Web.Policies.Requirements;

namespace Smart.FA.Catalog.Web.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCatalogDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddScoped<IUserIdentity, UserIdentity>()
            .AddOptions(configuration)
            .AddAuthorizationHandlers();
    }

    private static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .Configure<AdminOptions>(configuration.GetSection(AdminOptions.SectionName))
            .Configure<MediatROptions>(configuration.GetSection(MediatROptions.SectionName))
            .Configure<SuperUserOptions>(configuration.GetSection(SuperUserOptions.SectionName));
    }

    private static IServiceCollection AddAuthorizationHandlers(this IServiceCollection services)
    {
        return services
            .AddScoped<IAuthorizationHandler, UserChartRevisionApprovalHandler>();
    }

    public static IServiceCollection AddCatalogAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.AccessDeniedPath = new PathString("/UserChart");
            });
        return services;
    }

    public static IServiceCollection AddCatalogAuthorization(this IServiceCollection services)
    {
        return services.AddAuthorization(options =>
        {
            options.AddPolicy(List.AtLeastOneValidUserChartRevisionApproval,
                policy => policy.Requirements.Add(new AtLeastOneValidUserChartRevisionApprovalRequirement()));

            options.AddPolicy(List.MustBeSuperUser, policy => policy.RequireRole("SuperUser"));
        });
    }
}
