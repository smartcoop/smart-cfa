using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Smart.FA.Catalog.Showcase.Infrastructure.Data;

namespace Smart.FA.Catalog.Infrastructure;
public class DesignTimeContextFactory : IDesignTimeDbContextFactory<CatalogShowcaseContext>
{
    public CatalogShowcaseContext CreateDbContext(string[]? args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<CatalogShowcaseContext>();

        optionsBuilder.UseSqlServer("DataSource=app.db");
        return new CatalogShowcaseContext(optionsBuilder.Options);

    }
}
