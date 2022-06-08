using Dapper;
using Microsoft.Data.SqlClient;
using Smart.FA.Catalog.Core.Domain.Dto;
using Smart.FA.Catalog.Core.Domain.Interfaces;

namespace Smart.FA.Catalog.Infrastructure.Persistence.Read;

public class TrainerQueries : ITrainerQueries
{
    private readonly string _connectionString;

    public TrainerQueries(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<TrainerDto>> GetListAsync(List<int> trainingIds, CancellationToken cancellationToken)
    {
        var sql = @"SELECT
                       Id,
                       FirstName,
                       LastName,
                       Biography,
                       Title,
                       DefaultLanguage
                    FROM Cfa.Trainer T
                    INNER JOIN Cfa.TrainerAssignment TE ON T.Id = TE.TrainerId
                    WHERE TE.TrainingId IN @TrainingIds";
        await using var connection = new SqlConnection(_connectionString);
        var test =  connection.Query<TrainerDto>(sql, new {TrainingIds = trainingIds});
        return test;
    }
}
