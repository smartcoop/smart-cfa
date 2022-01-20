using Core.SeedWork;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructure;
public class DesignTimeContextFactory : IDesignTimeDbContextFactory<Context>
{
    public Context CreateDbContext(string[] args)
    {
        var basePath = Directory.GetCurrentDirectory();
        var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        Console.WriteLine($"\nDesignTimeContextFactory.Create(string[]):\n\tBase Path: {basePath}\n\tEnvironmentVariable: {environmentName}");
        return Create(basePath, environmentName, true);
    }

    private static Context Create(string basePath, string environmentName, bool useConsoleLogger)
    {
        const string appSettingsFileName = "appsettings";
        var builder = new ConfigurationBuilder()
                     .SetBasePath(basePath)
                     .AddJsonFile($"{appSettingsFileName}.json", false)
                     .AddJsonFile($"{appSettingsFileName}.{environmentName}.json", true)
                     .AddJsonFile($"{appSettingsFileName}.Local.json", true)
                     .AddEnvironmentVariables();

        var config = builder.Build();
        var connectionString = config.GetConnectionString("Training");
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException(
                "Could not find a connection string named 'Training'.");

        var optionsBuilder = new DbContextOptionsBuilder<Context>();
        Console.WriteLine($"Setting provider");

        optionsBuilder.UseSqlServer(connectionString);
        Console.WriteLine($"\nDesignTimeContextFactory.Create(string):\n\tConnection string: {connectionString}\n");
        var options = optionsBuilder.Options;
        return new Context(connectionString, useConsoleLogger, null);
    }
}
