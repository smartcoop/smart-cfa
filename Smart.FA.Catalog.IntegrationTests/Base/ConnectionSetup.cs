using System.IO;
using System.Reflection;
using Microsoft.Data.SqlClient;

namespace Smart.FA.Catalog.IntegrationTests.Base;

public class ConnectionSetup
{
    public const string DatabaseName = "Training";

    public static SqlConnectionStringBuilder Master =>
        new SqlConnectionStringBuilder
        {
            DataSource = @"(LocalDB)\MSSQLLocalDB",
            InitialCatalog = "master",
            IntegratedSecurity = true
        };

    public static SqlConnectionStringBuilder Training =>
        new SqlConnectionStringBuilder
        {
            DataSource = @"(LocalDB)\MSSQLLocalDB",
            InitialCatalog = "Training",
            IntegratedSecurity = true
        };

    public static string Filename => Path.Combine(
        Path.GetDirectoryName(
            typeof(TestSetup).GetTypeInfo().Assembly.Location)!,
        $"{DatabaseName}.mdf");
}
