using Core.Domain.Interfaces;
using Core.SeedWork;
using Core.Services;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Database;
using Infrastructure.Persistence.Read;
using Infrastructure.Persistence.Write;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, string connectionString, bool useConsoleLogger, IConfigurationSection mailOptionSection)
    {
        services.AddContext(connectionString, useConsoleLogger)
            .AddRepositories()
            .AddServices(mailOptionSection)
            .AddQueries(connectionString);
    }

    private static IServiceCollection AddContext(this IServiceCollection services, string connectionString, bool useConsoleLogger)
    {
        services.AddScoped(_ => new Context(connectionString, useConsoleLogger));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services, IConfigurationSection mailOptionSection)
    {
        services.Configure< MailOptions >( mailOptionSection );
        services.AddScoped<IMailService, MailService>();

        return services;
    }

    public static IServiceCollection AddQueries(this IServiceCollection services, string connectionString)
    {
        services.AddScoped<ITrainerQueries>(_ => new TrainerQueries(connectionString));
        services.AddScoped<ITrainingQueries>(_ => new TrainingQueries(connectionString));

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ITrainingRepository, TrainingRepository>();
        services.AddScoped<ITrainerRepository, TrainerRepository>();

        return services;
    }
}
