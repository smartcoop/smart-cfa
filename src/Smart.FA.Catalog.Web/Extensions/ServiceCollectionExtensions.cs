using Api.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Extensions;

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
