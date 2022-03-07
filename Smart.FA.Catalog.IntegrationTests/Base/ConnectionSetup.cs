using System.IO;
using System.Reflection;
using Microsoft.Data.SqlClient;

namespace Smart.FA.Catalog.IntegrationTests.Base;

public class ConnectionSetup
{
    public const string DatabaseName = "Catalog";

    public static SqlConnectionStringBuilder Master =>
        new SqlConnectionStringBuilder
        {
            DataSource = @"(localdb)\MSSQLLocalDb",
            InitialCatalog = "master",
            IntegratedSecurity = true
        };

    public static SqlConnectionStringBuilder Catalog =>
        new SqlConnectionStringBuilder
        {
            DataSource = @"(localdb)\MSSQLLocalDb",
            InitialCatalog = "Catalog",
            IntegratedSecurity = true
        };

    public static string Filename => Path.Combine(
        Path.GetDirectoryName(
            typeof(TestSetup).GetTypeInfo().Assembly.Location)!,
        $"{DatabaseName}.mdf");
}
