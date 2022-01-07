using Core.Domain.Dto;
using Core.Domain.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Infrastructure.Persistence.Read;

public class TrainingQueries: ITrainingQueries
{
    private readonly string _connectionString;

    public TrainingQueries(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<TrainingDto> FindAsync(int trainingId, string language, CancellationToken cancellationToken)
    {
        var sql = @"SELECT
	                    T.Id 'TrainingId',
	                    T.StatusId,
	                    TD.Title,
	                    TD.Goal
                    FROM dbo.Training T
                    INNER JOIN dbo.TrainingDetails TD ON T.Id = TD.TrainingId
                    WHERE TD.TrainingId = @TrainingId AND TD.Language = @Language";

        await using var db = new SqlConnection(_connectionString);
        return await db.QuerySingleAsync<TrainingDto>(sql, new {trainingId, language});
    }

    public async Task<IEnumerable<TrainingDto>> GetListAsync(int trainerId, string language, CancellationToken cancellationToken)
    {
        var sql = @"SELECT
	                    T.Id 'TrainingId',
	                    T.StatusId,
	                    TD.Title,
	                    TD.Goal,
                        TD.Language
                    FROM dbo.Training T
                    INNER JOIN dbo.TrainerEnrollment TE ON T.Id = TE.TrainingId
                    INNER JOIN dbo.TrainingDetails TD ON T.Id = TD.TrainingId
                    WHERE TE.TrainerId = @TrainerId AND TD.Language = @Language ";

        await using var db = new SqlConnection(_connectionString);
        return await db.QueryAsync<TrainingDto>(sql, new {trainerId, language});
    }
}
