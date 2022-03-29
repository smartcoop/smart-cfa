using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Smart.FA.Catalog.Core.Domain;
using Smart.FA.Catalog.Core.Domain.User.Enumerations;
using Smart.FA.Catalog.Core.Domain.ValueObjects;
using Smart.FA.Catalog.Core.Services;
using Smart.FA.Catalog.Infrastructure.Persistence;

namespace Smart.FA.Catalog.Infrastructure.Services;

/// <inheritdoc />
public class BootStrapService : IBootStrapService
{
    private readonly ILogger<BootStrapService> _logger;
    private readonly IServiceScopeFactory _factory;

    public BootStrapService(ILogger<BootStrapService> logger, IServiceScopeFactory factory)
    {
        _logger = logger;
        _factory = factory;
    }

    /// <inheritdoc />
    public async Task AddDefaultTrainerProfilePictureImage(string webRootPath)
    {
        using var serviceScope = _factory.CreateScope();
        var storageService = serviceScope.ServiceProvider.GetRequiredService<IS3StorageService>();
        var s3StorageOptions = serviceScope.ServiceProvider.GetRequiredService<IOptions<S3StorageOptions>>();
        var fileName = s3StorageOptions.Value.DefaultTrainerProfilePictureName;

        _logger.LogInformation("Seeding storage service with default image for trainer profile under the name {FileName}", fileName);

        var filePath = Path.Combine(webRootPath, "default_image.jpg");
        var file = File.OpenRead(filePath);

        await storageService.UploadAsync(file, fileName, CancellationToken.None);
    }

    /// <inheritdoc />
    public async Task ApplyMigrationsAndSeedAsync()
    {
        // We cannot inject Catalog context inside the constructor nor using the IServiceProvider.
        // The reason behind this is the CatalogContext has a scoped lifetime.
        // Therefore we need to achieve this by using a IServiceScope.
        using var serviceScope = _factory.CreateScope();
        var catalogContext = serviceScope.ServiceProvider.GetRequiredService<CatalogContext>();
        var currentConnection = catalogContext.Database.GetDbConnection();

        _logger.LogInformation("Seeding [{database}] database on SQL instance {server}", currentConnection.Database, currentConnection.DataSource);

        var completedWithSuccess = await SafeApplyMigrationsAndSeedWithRetriesAsync(catalogContext, currentConnection);

        if (!completedWithSuccess)
        {
            _logger.LogError("Seeding [{database}] database couldn't complete", currentConnection.Database);
            throw new Exception("Applying migrations and seed the database was not successful, aborting the startup of the application");
        }
    }

    /// <summary>
    /// Applies migrations and then seeds the database.
    /// </summary>
    /// <param name="catalogContext"><see cref="DbContext" /> on which the operations has to be performed.</param>
    /// <param name="currentConnection">Current connection of <paramref name="catalogContext"/>.</param>
    /// <returns>A task representing the asynchronous operation. The task's result is a boolean whose value tells if the operation was successful.</returns>
    private async Task<bool> SafeApplyMigrationsAndSeedWithRetriesAsync(CatalogContext catalogContext, IDbConnection currentConnection)
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
            await SeedDatabaseAsync(context);

            // Operation completed successfully
            return true;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Failure while applying migrations and seeding database.");
            return false;
        }
    }

    private async Task SeedDatabaseAsync(CatalogContext catalogContext)
    {
        await SeedTrainersAsync(catalogContext);
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
