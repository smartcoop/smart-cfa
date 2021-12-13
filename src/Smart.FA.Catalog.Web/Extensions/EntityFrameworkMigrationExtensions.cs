using Infrastructure.Persistence;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Api.Extensions;

public static class EntityFrameworkMigrationExtensions
{
    /// <summary>
    /// Apply Migrations in the Infrastructure when the data server is up and running.
    /// </summary>
    /// <param name="builder"></param>
    public static void ApplyMigrations(this WebApplicationBuilder builder)
    {
        ServiceProvider? services = builder.Services.BuildServiceProvider();
        for (int i = 0; i < 10; i++)
        {
            try
            {
                using var connection = new SqlConnection(builder.Configuration.GetConnectionString("Training"));
                using (var scope = services.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<Context>();
                    db.Database.Migrate();
                }
                break;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Thread.Sleep(20000);
        }
    }
}
