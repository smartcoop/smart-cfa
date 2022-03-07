using System.Data;
using Core.Domain;
using Core.Domain.Enumerations;
using Core.Services;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

/// <inheritdoc />
public class BootStrapService : IBootStrapService
{
    private readonly ILogger<BootStrapService> _logger;
    private readonly IServiceScopeFactory _factory;

    public BootStrapService(ILogger<BootStrapService> logger, IServiceScopeFactory factory)
    {
        _logger  = logger;
        _factory = factory;
    }

    /// <inheritdoc />
    public async Task ApplyMigrationsAndSeedAsync()
    {
        // We cannot inject Catalog context inside the constructor nor using the IServiceProvider.
        // The reason behind this is the CatalogContext has a scoped lifetime.
        // Therefore we need to achieve this by using a IServiceScope.
        using var serviceScope = _factory.CreateScope();
        var catalogContext     = serviceScope.ServiceProvider.GetRequiredService<CatalogContext>();
        var currentConnection  = catalogContext.Database.GetDbConnection();

        _logger.LogInformation("Seeding [{database}] database on SQL instance {server}", currentConnection.Database, currentConnection.DataSource);

        var completedWithSuccess = await SafeApplyMigrationsWithRetriesAsync(catalogContext, currentConnection);

        if (!completedWithSuccess)
        {
            _logger.LogError("Seeding [{database}] database couldn't complete", currentConnection.Database);
        }
    }

    private async Task<bool> SafeApplyMigrationsWithRetriesAsync(CatalogContext catalogContext, IDbConnection currentConnection)
    {
        int DelayToWaitBetweenRetriesInMilliseconds(int retryAttempt) => (int)(Math.Max(5 - retryAttempt, 0) + Math.Pow(2, Math.Min(retryAttempt, 5))) * 1_000;
        for (var retryAttempt = 0; retryAttempt < 6; retryAttempt++)
        {
            if (await SafeApplyMigrationsAndSeedAsync(catalogContext))
            {
                _logger.LogInformation("Seeding [{database}] database completed successfully", currentConnection.Database);
                return true;
            }

            // Arithmetic progression is: 6, 6, 7, 10, 17, 32 seconds. With 32 seconds being the maximum value.
            await Task.Delay(DelayToWaitBetweenRetriesInMilliseconds(retryAttempt));
        }

        return false;
    }

    private async Task<bool> SafeApplyMigrationsAndSeedAsync(CatalogContext context)
    {
        try
        {
            await context.Database.MigrateAsync();
            await SeedTrainersAsync(context);

            // Operation completed successfully
            return true;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Failure while applying migrations and seeding database.");
            return false;
        }
    }

    //TODO implementation an actual non hardcoded mechanism.
    private Task SeedTrainersAsync(CatalogContext catalogContext)
    {
        var trainer = new Trainer(Name.Create("Victor", "vD").Value,
            TrainerIdentity.Create("1", ApplicationType.Default).Value,
            "Developer",
            "Hello I am Victor van Duynen",
            Language.Create("FR").Value);
        catalogContext.Trainers.Add(trainer);
        return catalogContext.SaveChangesAsync();
    }
}
