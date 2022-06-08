using System;

namespace Smart.FA.Catalog.IntegrationTests.Base;

/// <summary>
/// The base class instantiated before each test run. It will recreate a fresh test db every time
/// </summary>
public class TestSetup : IntegrationTestBase, IDisposable
{
    public TestSetup()
    {
        DestroyDatabase();
        CreateDatabase();
    }

    public void Dispose()
    {
        DestroyDatabase();
    }

    private void CreateDatabase()
    {
        using (var context = GivenCatalogContext(beginTransaction: false))
        {
            context.Database.EnsureCreated();
        }
    }

    private void DestroyDatabase()
    {
        using (var context = GivenCatalogContext(beginTransaction: false))
        {
            context.Database.EnsureDeleted();
        }
    }
}
