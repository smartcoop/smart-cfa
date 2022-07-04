using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Smart.FA.Catalog.Infrastructure.Persistence;
using Smart.FA.Catalog.IntegrationTests.Mock;
using Smart.FA.Catalog.Tests.Common.Factories;

namespace Smart.FA.Catalog.IntegrationTests.Base;

/// <summary>
/// Base helper method that every integration tests needs, namely the DBContext <see cref="CatalogContext"/> and connection settings from <see cref="ConnectionSetup"/> for using Queries
/// Test classes need to inherit from the class in order to be able to use its methods
/// </summary>
public class IntegrationTestBase
{
    protected static ConnectionSetup Connection { get; private set; }

    protected static CatalogContext GivenCatalogContext(bool beginTransaction = true)
    {
        var dbOptions = CreateNewContextOptions();
        var context = new CatalogContext(dbOptions, MockedDomainEventPublisherFactory.Create(), MockedUserIdentityFactory.Create());
        if (beginTransaction)
            context.Database.BeginTransaction();
        return context;
    }

    private static DbContextOptions<CatalogContext> CreateNewContextOptions()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Environment.CurrentDirectory)
            .AddJsonFile("appsettings.Testing.json", false)
            .AddEnvironmentVariables();

        var config = builder.Build();

        Connection = new ConnectionSetup(config);

        Console.WriteLine(@"Setting SQL provider...");
        var optionsBuilder = new DbContextOptionsBuilder<CatalogContext>();
        optionsBuilder.UseSqlServer(Connection.Catalog.ConnectionString);
        var options = optionsBuilder.Options;

        return options;
    }
}
