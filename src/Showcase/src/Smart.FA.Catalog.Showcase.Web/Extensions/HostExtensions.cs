using Microsoft.EntityFrameworkCore;
using Smart.FA.Catalog.Showcase.Infrastructure.Data;

namespace Smart.FA.Catalog.Showcase.Web.Extensions;

public static class HostExtensions
{
    public static async Task BootStrapAsync(this IHost host)
    {
        await host.ApplyMigrationsAsync();
    }

    private static async Task ApplyMigrationsAsync(this IHost host)
    {
        var serviceProvider = host.Services;
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CatalogShowcaseContext>();
        await dbContext.Database.MigrateAsync();
    }
}
