using Hangfire;
using Hangfire.SqlServer;

namespace Smart.FA.Catalog.Showcase.Web.Extensions;

public static class HangfireServiceCollectionExtensions
{
    public static IServiceCollection AddHangfire(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddHangfire(globalConfiguration =>
            {
                globalConfiguration
                    .UseNLogLogProvider()
                    .UseSqlServerStorage(configuration.GetConnectionString("Hangfire"),
                        new SqlServerStorageOptions()
                        {
                            SchemaName = "CfaHangfire",
                            CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                            SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                            QueuePollInterval = TimeSpan.Zero,
                            UseRecommendedIsolationLevel = true,
                        })
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
#if DEBUG
                    .UseColouredConsoleLogProvider()
#endif
                    ;
            })
            .AddHangfireServer();
    }
}
