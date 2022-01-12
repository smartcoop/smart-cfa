using Infrastructure;
using Infrastructure.Persistence;

namespace Smart.FA.Catalog.IntegrationTests.Base;

public class IntegrationTestBase
{
    private static readonly DesignTimeContextFactory contextFactory = new();

    protected static Context GivenTrainingContext(bool beginTransaction = true)
    {
        var context = contextFactory.CreateDbContext(null);
        if (beginTransaction)
            context.Database.BeginTransaction();
        return context;
    }
}
