using System.Data;
using Core.Domain;
using Core.Domain.Dto;
using Core.Domain.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Infrastructure.Persistence.Read;

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
                       Description,
                       DefaultLanguage
                    FROM dbo.Trainer T
                    INNER JOIN dbo.TrainerEnrollment TE ON T.Id = TE.TrainerId
                    WHERE TE.TrainingId IN @TrainingIds";
        await using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<TrainerDto>(sql, new {TrainingIds = trainingIds});
    }
}
