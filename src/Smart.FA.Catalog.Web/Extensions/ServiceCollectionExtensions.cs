using Smart.FA.Catalog.Core.Services;
using Smart.FA.Catalog.Web.Identity;
using Smart.FA.Catalog.Web.Options;

namespace Smart.FA.Catalog.Web.Extensions;

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
