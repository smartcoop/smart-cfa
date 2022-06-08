using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
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

    private void CreateDatabase()
    {
        using (var context = GivenCatalogContext(beginTransaction: false))
        {
            if (!context.Database.CanConnect())
            {
                context.Database.EnsureCreated();
            }
            else
            {
                Console.WriteLine("Test");
            }
        }
    }

    private void DestroyDatabase()
    {
        using (var context = GivenCatalogContext(beginTransaction: false))
        {
            context.Database.EnsureDeleted();
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
