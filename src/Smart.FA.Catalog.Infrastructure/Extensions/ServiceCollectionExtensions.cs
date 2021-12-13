using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddContext(connectionString);
    }

    private static IServiceCollection AddContext(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<Context>(options => options
                                             .UseSqlServer(connectionString))
                                             .BuildServiceProvider();
        return services;
    }
}
