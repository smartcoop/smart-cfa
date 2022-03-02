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
    public async Task SeedAndApplyMigrationsAsync()
    {
        // we cannot inject Catalog context inside the constructor nor using the IServiceProvider.
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
        for (var i = 0; i < 150; i++)
        {
            if (await SafeApplyMigrationsAsync(catalogContext))
            {
                _logger.LogInformation("Seeding [{database}] database completed successfully", currentConnection.Database);
                return true;
            }

            await Task.Delay(1000);
        }

        return false;
    }

    private async Task<bool> SafeApplyMigrationsAsync(CatalogContext context)
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

    //TODO use an actual file such as an csv.
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
