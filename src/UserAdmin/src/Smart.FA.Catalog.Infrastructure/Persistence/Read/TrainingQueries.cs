using Dapper;
using Dapper.Transaction;
using Microsoft.Data.SqlClient;
using Smart.FA.Catalog.Core.Domain.Dto;
using Smart.FA.Catalog.Core.Domain.Interfaces;
using Smart.FA.Catalog.Core.SeedWork;

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
	                    T.StatusId,
	                    TD.Title,
	                    TD.Goal,
                        TD.Language
                    FROM dbo.Training T
                    INNER JOIN dbo.TrainingDetail TD ON T.Id = TD.TrainingId
                    WHERE TD.TrainingId = @TrainingId AND TD.Language = @Language";
        await using var connection = new SqlConnection(_connectionString);
        return await connection.QuerySingleAsync<TrainingDto>(sql, new {trainingId, language});
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
	                    T.StatusId,
	                    TD.Title,
	                    TD.Goal,
                        TD.Language,
                        TC.TrainingTopicId 'TopicId'
                    FROM dbo.Training T
                    INNER JOIN dbo.TrainerAssignment TE ON T.Id = TE.TrainingId
                    LEFT JOIN dbo.TrainingCategory TC ON T.Id = TC.TrainingId
                    INNER JOIN dbo.TrainingDetail TD ON T.Id = TD.TrainingId
                    WHERE TE.TrainerId = @TrainerId AND TD.Language = @Language ";
        await using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<TrainingDto, int?, TrainingDto>(sql, (dto, topicId) =>
            {
                if (topicId is not null)
                {
                    dto.TopicIds?.Add((int) topicId);
                }
                return dto;
            },
            splitOn: "TopicId",
            param: new {trainerId, language});
    }

    //TODO: it's a bit heavy for a simple paging system (though it works), we might want to look to refactor that at some point
    public async Task<PagedList<TrainingDto>> GetPagedListAsync(int trainerId, string language, PageItem pageItem,
        CancellationToken cancellationToken)
    {
        var sql = @"SELECT
	                    T.Id 'TrainingId',
	                    T.StatusId,
	                    TD.Title,
	                    TD.Goal,
                        TD.Language,
                        TC.TrainingTopicId 'TopicId'
                    FROM (SELECT * FROM dbo.Training T ORDER BY T.Id
                    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY) T
                    INNER JOIN dbo.TrainerAssignment TE ON T.Id = TE.TrainingId
                    LEFT JOIN dbo.TrainingCategory TC ON T.Id = TC.TrainingId
                    INNER JOIN dbo.TrainingDetail TD ON T.Id = TD.TrainingId
                    WHERE TE.TrainerId = @TrainerId AND TD.Language = @Language";

        var countsql = @"SELECT
	                        COUNT(*)
                        FROM (SELECT T.Id FROM dbo.Training T
                        INNER JOIN dbo.TrainerAssignment TE ON T.Id = TE.TrainingId
                        LEFT JOIN dbo.TrainingCategory TC ON T.Id = TC.TrainingId
                        INNER JOIN dbo.TrainingDetail TD ON T.Id = TD.TrainingId
                        WHERE TE.TrainerId = @TrainerId AND TD.Language = @Language
                        GROUP BY T.Id) A";

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);
        await using var transaction = connection.BeginTransaction();

        //Fetch the count of distinct trainings
        var count = await transaction.QuerySingleAsync<int>(countsql, new {trainerId, language});
        //Get a list of training and splits field on the training topic id
        var list = await transaction.QueryAsync<TrainingDto, int?, TrainingDto>(sql, (dto, topicId) =>
            {
                if (topicId is not null)
                {
                    dto.TopicIds?.Add((int) topicId);
                }

                return dto;
            },
            splitOn: "TopicId",
            param: new {trainerId, language, pageItem.Offset, pageItem.PageSize});
        transaction.Commit();
        //Regroup all training records by id
        var result = list.GroupBy(dto => dto.TrainingId).Select(trainingGroup =>
            new TrainingDto(trainingGroup.Key,
                trainingGroup.First().StatusId,
                trainingGroup.First().Title,
                trainingGroup.First().Goal,
                trainingGroup.First().Language,
                trainingGroup.SelectMany(tg => tg.TopicIds).ToList()));

        return new PagedList<TrainingDto>(result, pageItem, count);
    }
}
