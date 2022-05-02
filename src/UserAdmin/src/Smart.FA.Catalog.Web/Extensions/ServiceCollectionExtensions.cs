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
    public static void AddApi(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddScoped<IUserIdentity, UserIdentity>()
            .AddOptions(configuration);
    }

    private static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AdminOptions>(configuration.GetSection(AdminOptions.SectionName));
        services.Configure<MediatROptions>(configuration.GetSection(MediatROptions.SectionName));
        services.AddScoped<IAuthorizationHandler, UserChartRevisionApprovalHandler>();
        services.AddSingleton<IAuthorizationMiddlewareResultHandler, UserAdminAuthorizationResultHandler>();
        return services;
    }
}
