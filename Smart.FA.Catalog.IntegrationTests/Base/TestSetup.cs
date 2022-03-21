using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Smart.FA.Catalog.IntegrationTests.Base;

public class TestSetup : IntegrationTestBase, IDisposable
{
    public TestSetup()
    {
        DestroyDatabase();
        CreateDatabase();
    }

    public void Dispose()
    {
        DestroyDatabase();
    }

    private static void CreateDatabase()
    {
        ExecuteSqlCommand(ConnectionSetup.Master, $@"
                CREATE DATABASE [{ConnectionSetup.DatabaseName}]
                ON (NAME = '{ConnectionSetup.DatabaseName}',
                FILENAME = '{ConnectionSetup.Filename}')");

        using (var context = GivenCatalogContext(beginTransaction: false))
        {
            context.Database.Migrate();
            context.SaveChanges();
        }
    }

    private static void DestroyDatabase()
    {
        var fileNames = ExecuteSqlQuery(ConnectionSetup.Master, $@"
                SELECT [physical_name] FROM [sys].[master_files]
                WHERE [database_id] = DB_ID('{ConnectionSetup.DatabaseName}')",
            row => (string)row["physical_name"]);

        if (fileNames.Any())
        {
            ExecuteSqlCommand(ConnectionSetup.Master, $@"
                    ALTER DATABASE [{ConnectionSetup.DatabaseName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                    EXEC sp_detach_db '{ConnectionSetup.DatabaseName}'");

            fileNames.ForEach(File.Delete);
        }
    }

    private static List<T> ExecuteSqlQuery<T>(
    SqlConnectionStringBuilder connectionStringBuilder,
    string queryText,
    Func<SqlDataReader, T> read)
    {
        var result = new List<T>();
        using (var connection = new SqlConnection(connectionStringBuilder.ConnectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = queryText;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(read(reader));
                    }
                }
            }
        }
        return result;
    }

    private static void ExecuteSqlCommand(
      SqlConnectionStringBuilder connectionStringBuilder,
      string commandText)
    {
        using (var connection = new SqlConnection(connectionStringBuilder.ConnectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = commandText;
                command.ExecuteNonQuery();
            }
        }
    }
}
