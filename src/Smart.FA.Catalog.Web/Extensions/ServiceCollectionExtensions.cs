using Application.Interceptors;
using Core.Services;
using MediatR;
using Web.Extensions.Middlewares;
using Web.Identity;
using Web.Options;

namespace Web.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApi(this IServiceCollection services, IConfigurationSection adminOptionsSection)
    {
        services
            .AddScoped<IUserIdentity, UserIdentity>()
            .AddOptions(adminOptionsSection);
    }

    private static IServiceCollection AddOptions(this IServiceCollection services, IConfigurationSection adminOptionsSection)
    {
        return services.Configure<AdminOptions>(adminOptionsSection);
    }
}
