using Web.Options;

namespace Web.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApi(this IServiceCollection services, IConfigurationSection adminOptionsSection)
    {
        services
            .AddOptions(adminOptionsSection);
    }

    public static void AddOptions(this IServiceCollection services, IConfigurationSection adminOptionsSection)
    {
        services.Configure<AdminOptions>(adminOptionsSection);
    }
}
