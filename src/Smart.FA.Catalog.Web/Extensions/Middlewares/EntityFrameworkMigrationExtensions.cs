using Core.Domain;
using Core.Domain.Enumerations;
using Infrastructure.Persistence;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Api.Extensions.Middlewares;

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
                using var connection = new SqlConnection(builder.Configuration.GetConnectionString("Training"));
                using (var scope = services.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<Context>();
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

    public static void Seed(Context context)
    {
        var trainer = new Trainer(Name.Create("Victor", "vD").Value,
            TrainerIdentity.Create("1", ApplicationType.Default).Value, "Hello I am Victor van Duynen",
            Language.Create("FR").Value);
        context.Trainers.Add(trainer);
        context.SaveChanges();
    }
}
