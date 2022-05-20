using Microsoft.AspNetCore.Authorization;
using Smart.FA.Catalog.UserAdmin.Application.Models.Options;
using Smart.FA.Catalog.UserAdmin.Domain.Services;
using Smart.FA.Catalog.UserAdmin.Web.Authentication;
using Smart.FA.Catalog.UserAdmin.Web.Authentication.Handlers;
using Smart.FA.Catalog.UserAdmin.Web.Authentication.Header;
using Smart.FA.Catalog.UserAdmin.Web.Authorization.Handlers;
using Smart.FA.Catalog.UserAdmin.Web.Authorization.Policy;
using Smart.FA.Catalog.UserAdmin.Web.Authorization.Policy.Requirements;
using Smart.FA.Catalog.UserAdmin.Web.Identity;
using Smart.FA.Catalog.UserAdmin.Web.Options;

namespace Smart.FA.Catalog.UserAdmin.Web.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWebDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddScoped<IUserIdentity, UserIdentity>()
            .AddOptions(configuration);
    }

    private static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AdminOptions>(configuration.GetSection(AdminOptions.SectionName));
        services.Configure<MediatROptions>(configuration.GetSection(MediatROptions.SectionName));
        services.Configure<SuperUserOptions>(configuration.GetSection(SuperUserOptions.SectionName));;
        services.Configure<SpecialAuthenticationOptions>(configuration.GetSection(SpecialAuthenticationOptions.SectionName));
        services.Configure<UrlOptions>(configuration.GetSection(UrlOptions.UrlSectionName));

        return services;
    }

    public static IServiceCollection AddWebAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication(options => options.DefaultScheme = AuthSchemes.UserAdmin)
            .AddScheme<CfaAuthenticationOptions, UserAdminAuthenticationHandler>(AuthSchemes.UserAdmin, null);
        services.AddScoped<IAccountDataHeaderSerializer, AccountDataHeaderHeaderSerializer>();

        return services;
    }

    public static IServiceCollection AddWebAuthorization(this IServiceCollection services)
    {
        return services
            .AddAuthorization(ConfigureAuthorizationOptions)
            .AddAuthorizationHandlers();
    }

    public static IServiceCollection AddAuthorizationHandlers(this IServiceCollection services)
    {
        return services.AddSingleton<IAuthorizationMiddlewareResultHandler, UserAdminAuthorizationResultHandler>()
            .AddScoped<IAuthorizationHandler, MustBeSuperUserOrTrainingCreatorHandler>()
            .AddScoped<IAuthorizationHandler, UserChartRevisionApprovalHandler>();
    }

    private static void ConfigureAuthorizationOptions(this AuthorizationOptions options)
    {
        options.AddPolicy(Policies.AtLeastOneValidUserChartRevisionApproval,
            policy => policy.Requirements.Add(new AtLeastOneActiveUserChartRevisionApprovalRequirement()));

        options.AddPolicy(Policies.MustBeSuperUser, policy => policy.RequireRole("SuperUser"));

        options.AddPolicy(Policies.MustBeSuperUserOrTrainingCreator,
            policy => policy.Requirements.Add(new MustBeSuperUserOrTrainingCreator()));
    }
}
