using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Smart.FA.Catalog.Core.Domain;
using Smart.FA.Catalog.Core.Domain.Models;
using Smart.FA.Catalog.Core.Domain.User.Enumerations;
using Smart.FA.Catalog.Core.Domain.ValueObjects;
using Smart.FA.Catalog.Core.Services;
using Smart.FA.Catalog.Infrastructure.Persistence;
using Smart.FA.Catalog.Tests.Common;

namespace Smart.FA.Catalog.IntegrationTests.Base;

public class MockedUserIdentity : IUserIdentity
{
    public int Id { get; } = 1;
    public CustomIdentity Identity { get; } = new(new(Name.Create("Vic", "vD").Value, TrainerIdentity.Create("1", ApplicationType.Default).Value, "Test", "Test", Language.Create("FR").Value));
    public Trainer CurrentTrainer { get; }
    public bool IsSuperUser { get; }
}

public class IntegrationTestBase
{
    public static ConnectionSetup Connection { get; private set; }

    protected static CatalogContext GivenCatalogContext(bool beginTransaction = true)
    {
        var dbOptions = CreateNewContextOptions();
        var context = new CatalogContext(dbOptions, DomainEventPublisherFactory.Create(), new MockedUserIdentity());
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
        var connectionString = config.GetConnectionString("Catalog");
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException(
                "Could not find a connection string named 'Catalog'.");

        var optionsBuilder = new DbContextOptionsBuilder<CatalogContext>();
        Console.WriteLine("Setting provider");

        optionsBuilder.UseSqlServer(connectionString);
        var options = optionsBuilder.Options;

        return options;
    }
}
