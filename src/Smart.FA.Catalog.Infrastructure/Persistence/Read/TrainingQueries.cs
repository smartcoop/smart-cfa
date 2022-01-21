using Core.Domain.Dto;
using Core.Domain.Interfaces;
using Core.SeedWork;
using Dapper;
using Dapper.Transaction;
using Microsoft.Data.SqlClient;

namespace Infrastructure.Persistence.Read;

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

    public async Task<IEnumerable<TrainingDto>> GetListAsync(int trainerId, string language,
        CancellationToken cancellationToken)
    {
        var sql = @"SELECT
	                    T.Id 'TrainingId',
	                    T.StatusId,
	                    TD.Title,
	                    TD.Goal,
                        TD.Language
                    FROM dbo.Training T
                    INNER JOIN dbo.TrainerEnrollment TE ON T.Id = TE.TrainingId
                    INNER JOIN dbo.TrainingDetail TD ON T.Id = TD.TrainingId
                    WHERE TE.TrainerId = @TrainerId AND TD.Language = @Language ";
        await using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<TrainingDto>(sql, new {trainerId, language});
    }

    public async Task<PagedList<TrainingDto>> GetPagedListAsync(int trainerId, string language, PageItem pageItem,
        CancellationToken cancellationToken)
    {
        var sql = @"SELECT
	                    T.Id 'TrainingId',
	                    T.StatusId,
	                    TD.Title,
	                    TD.Goal,
                        TD.Language
                    FROM dbo.Training T
                    INNER JOIN dbo.TrainerEnrollment TE ON T.Id = TE.TrainingId
                    INNER JOIN dbo.TrainingDetail TD ON T.Id = TD.TrainingId
                    WHERE TE.TrainerId = @TrainerId AND TD.Language = @Language
                    ORDER BY T.Id
                    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

        var countsql = @"SELECT
	                    COUNT(*)
                    FROM dbo.Training T
                    INNER JOIN dbo.TrainerEnrollment TE ON T.Id = TE.TrainingId
                    INNER JOIN dbo.TrainingDetail TD ON T.Id = TD.TrainingId
                    WHERE TE.TrainerId = @TrainerId AND TD.Language = @Language";

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);
        await using var transaction = connection.BeginTransaction();
        var count = await transaction.QuerySingleAsync<int>(countsql, new {trainerId, language});
        var list = await transaction.QueryAsync<TrainingDto>(sql, new {trainerId, language, pageItem.Offset, pageItem.PageSize});
        transaction.Commit();
        return new PagedList<TrainingDto>(list, pageItem, count);
    }
}
