using Core.Domain;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Infrastructure.Persistence.Read;

public class TrainerQueries
{
    private readonly string _connectionString;

    public TrainerQueries(string connectionString)
    {
        _connectionString = connectionString;
    }
    public async Task< IEnumerable<Trainer>> GetTrainingsByTrainerId(int trainerId)
    {
        var sql = @"SELECT FirstName, LastName, Description, DefaultLanguage FROM ";
        await using var db = new SqlConnection(_connectionString);
        return await db.QueryAsync< Trainer >( sql, new { trainerId } );

    }
}
