using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLog.Extensions.Logging;
using Smart.FA.Catalog.Core.Domain.Interfaces;
using Smart.FA.Catalog.Core.SeedWork;
using Smart.FA.Catalog.Core.Services;
using Smart.FA.Catalog.Infrastructure.Options;
using Smart.FA.Catalog.Infrastructure.Persistence;
using Smart.FA.Catalog.Infrastructure.Persistence.Database;
using Smart.FA.Catalog.Infrastructure.Persistence.Read;
using Smart.FA.Catalog.Infrastructure.Persistence.Write;
using Smart.FA.Catalog.Infrastructure.Services;
using Smart.FA.Catalog.Infrastructure.Services.Options;

namespace Smart.FA.Catalog.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructure
    (
        this IServiceCollection services
        , string catalogConnectionString
        , string userAccountConnectionString
        , IConfigurationSection mailOptionSection
        , IConfigurationSection dalOptionSection
    )
    {
        services
            .AddEventPublisher()
            .AddRepositories()
            .AddServices(userAccountConnectionString, mailOptionSection)
            .AddQueries(catalogConnectionString)
            .AddDbContext(catalogConnectionString, dalOptionSection);
    }

    private static IServiceCollection AddEventPublisher(this IServiceCollection services)
    {
        return services.AddScoped<IDomainEventPublisher, DomainEventPublisher>();
    }

    private static IServiceCollection AddServices(this IServiceCollection services, string userAccountConnectionString,
        IConfigurationSection mailOptionSection)
    {
        services.Configure<MailOptions>(mailOptionSection);
        services.AddScoped(_ => new UserStrategyResolver(userAccountConnectionString));
        services.AddScoped<IMailService, MailService>();
        services.AddScoped<IBootStrapService, BootStrapService>();
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

    private static IServiceCollection AddDbContext(this IServiceCollection services, string connectionString,
        IConfigurationSection efCoreSection)
    {
        services.Configure<EFCore>(efCoreSection);
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddDbContext<CatalogContext>((serviceProvider, options) =>
        {
            var efCoreOptions = serviceProvider.GetRequiredService<IOptions<EFCore>>();
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddFilter((category, level) =>
                        category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information)
                    .AddNLog()
                    .AddConsole();
            });

            options
                .UseSqlServer(connectionString)
                .UseLazyLoadingProxies();
            if (efCoreOptions.Value.UseConsoleLogger)
            {
                options
                    .UseLoggerFactory(loggerFactory)
                    .EnableSensitiveDataLogging();
            }
        });
        return services;
    }
}
