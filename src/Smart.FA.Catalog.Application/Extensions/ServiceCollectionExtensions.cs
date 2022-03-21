using System.Reflection;
using Application.Interceptors;
using Application.SeedWork;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions;

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
