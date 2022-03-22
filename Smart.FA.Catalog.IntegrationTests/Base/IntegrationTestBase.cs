using Infrastructure;
using Smart.FA.Catalog.Infrastructure;
using Smart.FA.Catalog.Infrastructure.Persistence;

namespace Smart.FA.Catalog.IntegrationTests.Base;

public class IntegrationTestBase
{
    private static readonly DesignTimeContextFactory ContextFactory = new();

    protected static CatalogContext GivenTrainingContext(bool beginTransaction = true)
    {
        var context = ContextFactory.CreateDbContext(null);
        if (beginTransaction)
            context.Database.BeginTransaction();
        return context;
    }
}
