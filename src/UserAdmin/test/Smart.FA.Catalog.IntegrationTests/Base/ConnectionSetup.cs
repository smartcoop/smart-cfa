using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Smart.FA.Catalog.IntegrationTests.Base;

/// <summary>
/// Includes all connection strings for databases needed for integration testing
/// </summary>
public class ConnectionSetup
{
    private readonly IConfigurationRoot _configuration;

    public ConnectionSetup(IConfigurationRoot configuration)
    {
        _configuration = configuration;
    }

    public SqlConnectionStringBuilder Master =>
        new(_configuration.GetConnectionString("Catalog")) { InitialCatalog = "master" };

    public SqlConnectionStringBuilder Catalog =>
        new(_configuration.GetConnectionString("Catalog"));
}
