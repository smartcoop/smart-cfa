using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Smart.FA.Catalog.Core.Domain.Interfaces;
using Smart.FA.Catalog.Core.SeedWork;
using Smart.FA.Catalog.Core.Services;
using Smart.FA.Catalog.Infrastructure.Persistence;
using Smart.FA.Catalog.Infrastructure.Persistence.Database;
using Smart.FA.Catalog.Infrastructure.Persistence.Read;
using Smart.FA.Catalog.Infrastructure.Persistence.Write;
using Smart.FA.Catalog.Infrastructure.Services;
using Smart.FA.Catalog.Infrastructure.Services.Options;

namespace Smart.FA.Catalog.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, string trainingConnectionString,
        string userAccountConnectionString,
        bool useConsoleLogger, IConfigurationSection mailOptionSection)
    {
        services.AddContext(trainingConnectionString, useConsoleLogger)
            .AddEventDispatcher()
            .AddRepositories()
            .AddServices(userAccountConnectionString, mailOptionSection)
            .AddQueries(trainingConnectionString);
    }

    private static IServiceCollection AddEventDispatcher(this IServiceCollection services)
    {
        services.AddScoped<IBus, Bus>();
        services.AddScoped<MessageBus>();
        services.AddScoped<EventDispatcher>();
        return services;
    }

    private static IServiceCollection AddContext(this IServiceCollection services, string connectionString,
        bool useConsoleLogger)
    {
        services.AddScoped(provider =>
            new CatalogContext(connectionString, useConsoleLogger, provider.GetRequiredService<EventDispatcher>()));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services, string userAccountConnectionString,
        IConfigurationSection mailOptionSection)
    {
        services.Configure<MailOptions>(mailOptionSection);
        services.AddScoped(_ => new UserStrategyResolver(userAccountConnectionString));
        services.AddScoped<IMailService, MailService>();

        services.AddTransient<IBootStrapService, BootStrapService>();

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ITrainingRepository, TrainingRepository>();
        services.AddScoped<ITrainerRepository, TrainerRepository>();

        return services;
    }

    private static IServiceCollection AddQueries(this IServiceCollection services, string connectionString)
    {
        //Note: Dapper handles disposing of connections, so using statement are not necessary for the IDbConnection
        services.AddScoped<ITrainerQueries>(_ => new TrainerQueries(connectionString));
        services.AddScoped<ITrainingQueries>(_ => new TrainingQueries(connectionString));

        return services;
    }
}
