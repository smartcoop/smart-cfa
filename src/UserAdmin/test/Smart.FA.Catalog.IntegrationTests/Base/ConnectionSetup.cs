using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Smart.FA.Catalog.IntegrationTests.Base;

public class ConnectionSetup
{
    private readonly IConfigurationRoot _configuration;
    private const string DatabaseName = "Catalog";

    public ConnectionSetup(IConfigurationRoot configuration)
    {
        _configuration = configuration;
    }

    public SqlConnectionStringBuilder Master =>
        new() { DataSource = Catalog.DataSource, InitialCatalog = "master", IntegratedSecurity = true };

    public SqlConnectionStringBuilder Catalog =>
        new(_configuration.GetConnectionString(DatabaseName));
}
