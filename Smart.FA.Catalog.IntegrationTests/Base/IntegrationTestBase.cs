using Infrastructure;
using Infrastructure.Persistence;

namespace Smart.FA.Catalog.IntegrationTests.Base;

public class IntegrationTestBase
{
    private static readonly DesignTimeContextFactory contextFactory = new();

    protected static Context GivenTrainingContext(bool beginTransaction = true)
    {
        var context = contextFactory.CreateDbContext(null);
        if (beginTransaction)
            context.Database.BeginTransaction();
        return context;
    }

    #region Define custom data provider (commented out)
    //protected static Context GivenTrainingContext (bool beginTransaction = true)
    //{
    //    var context = new Context(new DbContextOptionsBuilder<Context>()
    //        .UseSqlServer(Training.ConnectionString)
    //        .Options
    //        );
    //    if(beginTransaction)
    //          context.Database.BeginTransaction();
    //    return context;
    //}
    //private static SqlConnectionStringBuilder Training =>
    //       new SqlConnectionStringBuilder
    //       {
    //           DataSource = @"(Localdb)\MSSQLLocalDB",
    //           InitialCatalog = "Training",
    //           IntegratedSecurity = true
    //       }; 
    #endregion
}
