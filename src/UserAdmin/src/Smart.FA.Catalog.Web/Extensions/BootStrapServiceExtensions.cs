using Smart.FA.Catalog.Application.Extensions;
using Smart.FA.Catalog.Core.Services;
using Smart.FA.Catalog.Infrastructure.Services;
using Smart.FA.Catalog.Web.Security;

namespace Smart.FA.Catalog.Web.Extensions;

public static class BootStrapServiceExtensions
{
    public static async Task ExecuteBootStrapService(this IApplicationBuilder builder)
    {
        var migrationScope = builder.ApplicationServices.CreateScope();
        // Fix an issue with kerberos authentication on a linux server
        NetSecurityNativeFix.Initialize(migrationScope.ServiceProvider.GetRequiredService<ILogger<BootStrapService>>());
        var bootstrapService = migrationScope.ServiceProvider.GetRequiredService<IBootStrapService>();
        var environment = builder.ApplicationServices.GetRequiredService<IWebHostEnvironment>();
        if (!(environment.IsEnvironment("PreProduction") || environment.IsProduction()))
        {
            await bootstrapService.ApplyMigrationsAndSeedAsync();
        }
        await bootstrapService.AddDefaultTrainerProfilePictureImage(environment.WebRootPath);
        await bootstrapService.AddDefaultUserChart(environment.WebRootPath);
    }
}
