using Smart.FA.Catalog.Shared.Security;
using Smart.FA.Catalog.UserAdmin.Domain.Services;
using Smart.FA.Catalog.UserAdmin.Infrastructure.Services;

namespace Smart.FA.Catalog.UserAdmin.Web.Extensions;

public static class BootStrapServiceExtensions
{
    public static async Task ExecuteBootStrapService(this IApplicationBuilder builder)
    {
        var migrationScope = builder.ApplicationServices.CreateScope();
        // A fix which allows SQL domain authentication with Kerberos on linux host
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
