using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Smart.FA.Catalog.UserAdmin.Application.Interceptors;

namespace Smart.FA.Catalog.UserAdmin.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplication(this IServiceCollection services)
    {
        services
            .AddMediatR(Assembly.GetExecutingAssembly())
            .AddMediatrPipelineBehaviours();
    }

    private static IServiceCollection AddMediatrPipelineBehaviours(this IServiceCollection services)
    {
        return services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingPipelineBehavior<,>));
    }
}
