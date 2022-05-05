using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Smart.FA.Catalog.Infrastructure.Persistence;

namespace Smart.FA.Catalog.Infrastructure;
public class DesignTimeContextFactory : IDesignTimeDbContextFactory<CatalogContext>
{
    public CatalogContext CreateDbContext(string[]? args)
    {
        var basePath = Directory.GetCurrentDirectory();
        var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        Console.WriteLine($"\nDesignTimeContextFactory.Create(string[]):\n\tBase Path: {basePath}\n\tEnvironmentVariable: {environmentName}");
        return Create(basePath, environmentName!, true);
    }

    private static CatalogContext Create(string basePath, string environmentName, bool useConsoleLogger)
    {
        const string appSettingsFileName = "appsettings.Infrastructure";
        var builder = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile($"{appSettingsFileName}.json", false)
            .AddJsonFile($"{appSettingsFileName}.{environmentName}.json", true)
            .AddJsonFile($"{appSettingsFileName}.Local.json", true)
            .AddEnvironmentVariables();

        var config = builder.Build();
        var connectionString = config.GetConnectionString("Catalog");
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException(
                "Could not find a connection string named 'Catalog'.");

        var optionsBuilder = new DbContextOptionsBuilder<CatalogContext>();
        Console.WriteLine($"Setting provider");

        optionsBuilder.UseSqlServer(connectionString);
        Console.WriteLine($"\nDesignTimeContextFactory.Create(string):\n\tConnection string: {connectionString}\n");
        var options = optionsBuilder.Options;
        return new CatalogContext(options, default, default);
    }
}
