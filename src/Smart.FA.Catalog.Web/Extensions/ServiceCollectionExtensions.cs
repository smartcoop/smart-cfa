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

        services.AddScoped<IUserIdentity, UserIdentity>();
        services.AddOptions(adminOptionsSection);
        services.AddCustomMediatR();
    }

    public static IServiceCollection AddOptions(this IServiceCollection services, IConfigurationSection adminOptionsSection)
    {
        services.Configure<AdminOptions>(adminOptionsSection);
        return services;
    }

    public static IServiceCollection AddCustomMediatR(this IServiceCollection services)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(HandlerLoggerPipelineBehavior<,>));
        return services;
    }
}
