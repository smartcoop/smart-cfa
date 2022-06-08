using Dapper;
using Dapper.Transaction;
using Microsoft.Data.SqlClient;
using Smart.FA.Catalog.Core.Domain.Dto;
using Smart.FA.Catalog.Core.Domain.Interfaces;
using Smart.FA.Catalog.Shared.Collections;

namespace Smart.FA.Catalog.Infrastructure.Persistence.Read;

public class TrainingQueries : ITrainingQueries
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
	                    T.TrainingStatusTypeId,
	                    TD.Title,
	                    TD.Goal,
                        TD.Language
                    FROM Cfa.Training T
                    INNER JOIN Cfa.TrainingLocalizedDetails TD ON T.Id = TD.TrainingId
                    WHERE TD.TrainingId = @TrainingId AND TD.Language = @Language";
        await using var connection = new SqlConnection(_connectionString);
        return await connection.QuerySingleAsync<TrainingDto>(sql, new { trainingId, language });
    }

    public async Task<IEnumerable<TrainingDto>> GetListAsync
    (
        int trainerId
        , string language
        , CancellationToken cancellationToken
    )
    {
        var sql = @"SELECT
	                    T.Id 'TrainingId',
	                    T.TrainingStatusTypeId,
	                    TD.Title,
	                    TD.Goal,
                        TD.Language,
                        TC.TopicId
                    FROM Cfa.Training T
                    INNER JOIN Cfa.TrainerAssignment TE ON T.Id = TE.TrainingId
                    LEFT JOIN Cfa.TrainingTopic TC ON T.Id = TC.TrainingId
                    INNER JOIN Cfa.TrainingLocalizedDetails TD ON T.Id = TD.TrainingId
                    WHERE TE.TrainerId = @TrainerId AND TD.Language = @Language ";
        await using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<TrainingDto, int?, TrainingDto>(sql, (dto, topicId) =>
            {
                if (topicId is not null)
                {
                    dto.TopicIds?.Add((int)topicId);
                }

                return dto;
            },
            splitOn: "TopicId",
            param: new { trainerId, language });
    }
}
