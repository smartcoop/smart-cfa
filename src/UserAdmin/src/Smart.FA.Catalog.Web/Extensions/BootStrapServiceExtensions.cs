using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Smart.FA.Catalog.Application.Extensions;
using Smart.FA.Catalog.Core.Services;

namespace Smart.FA.Catalog.Web.Extensions;

public static class BootStrapServiceExtensions
{
    public static async Task ExecuteBootStrapService(this IApplicationBuilder builder)
    {
        var migrationScope  = builder.ApplicationServices.CreateScope();
        var bootstrapService = migrationScope.ServiceProvider.GetRequiredService<IBootStrapService>();
        var environment = builder.ApplicationServices.GetRequiredService<IWebHostEnvironment>();
        await bootstrapService.ApplyMigrationsAndSeedAsync();
        await bootstrapService.AddDefaultTrainerProfilePictureImage(environment.WebRootPath);
        await bootstrapService.AddDefaultUserChart(environment.WebRootPath, UserChartExtensions.GenerateUserChartNameDefault());
    }
}
