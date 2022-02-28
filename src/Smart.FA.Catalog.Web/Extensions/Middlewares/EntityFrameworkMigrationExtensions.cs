using Core.Domain;
using Core.Domain.Enumerations;
using Infrastructure.Persistence;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Web.Extensions.Middlewares;

public static class EntityFrameworkMigrationExtensions
{
    //TODO: Find a better place to apply migrations to decouple Infra logic entirely
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
                using var connection = new SqlConnection(builder.Configuration.GetConnectionString("Catalog"));
                using (var scope = services.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<CatalogContext>();
                    Console.WriteLine($"trying to connect to {connection.DataSource}...");
                    context.Database.Migrate();
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

    public static void Seed(CatalogContext catalogContext)
    {
        var trainer = new Trainer(Name.Create("Victor", "vD").Value,
            TrainerIdentity.Create("1", ApplicationType.Default).Value,"Developer", "Hello I am Victor van Duynen",
            Language.Create("FR").Value);
        catalogContext.Trainers.Add(trainer);
        catalogContext.SaveChanges();
    }
}
