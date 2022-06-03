using Smart.FA.Catalog.Shared.Collections;
using Smart.FA.Catalog.Shared.Domain.Enumerations.Training;
using Smart.FA.Catalog.Showcase.Domain.Models;

namespace Smart.FA.Catalog.Showcase.Infrastructure.Data.Extensions;

public static class TrainingQueryableExtensions
{
    /// <summary>
    /// Retrieves a paginated list of <see cref="TrainingList" />.
    /// </summary>
    /// <param name="query"><see cref="IQueryable{TrainingList}"/> on which the operation will be performed.</param>
    /// <param name="pageNumber">The number of the page.</param>
    /// <param name="pageSize">Every <see cref="TrainingList" /> for a given number of trainings.</param>
    /// <param name="randomIds">Bool that indicates if the result should be randomly picked.</param>
    /// <returns>A task representing the asynchronous operations. The task's result is a paginated list of <see cref="TrainingList" />.</returns>
    public static async Task<PagedList<TrainingList>> GetPaginatedTrainingListsAsync(this IQueryable<TrainingList> query,
        int pageNumber,
        int pageSize,
        bool randomIds = false)
    {
        // We need only published trainings.
        var result = await query.PaginateAsync(pageNumber, pageSize, randomIds);
        return new PagedList<TrainingList>(result.PaginatedItems, new PageItem(pageNumber, pageSize), result.TotalCount);
    }

    public static async Task<PagedList<TrainingList>> GetPaginatedTrainingListsByTrainerIdAsync(this IQueryable<TrainingList> query,
        int trainerId,
        int pageNumber,
        int pageSize,
        bool randomIds = false)
    {
        // We need only published trainings.
        query = query.Where(training => training.Status == TrainingStatusType.Published.Id && training.TrainerId == trainerId);
        var result = await query.PaginateAsync(pageNumber, pageSize, randomIds);
        return new PagedList<TrainingList>(result.PaginatedItems, new PageItem(pageNumber, pageSize), result.TotalCount);
    }

    public static async Task<PagedList<TrainingList>> SearchPaginatedTrainingsAsync(this IQueryable<TrainingList> query,
        string? searchKeyWord,
        int pageNumber,
        int pageSize)
    {
        if (!string.IsNullOrEmpty(searchKeyWord))
        {
            query = query.Where(trainingList => trainingList.Title.Contains(searchKeyWord) ||
                                                trainingList.Goal.Contains(searchKeyWord) ||
                                                trainingList.Methodology.Contains(searchKeyWord));
        }

        var paginationResult = await query.PaginateAsync(pageNumber, pageSize);
        return new PagedList<TrainingList>(paginationResult.PaginatedItems, new PageItem(pageNumber, pageSize), paginationResult.TotalCount);
    }

    public static async Task<PagedList<TrainingList>> SearchPaginatedTrainingsByTopicAsync(this IQueryable<TrainingList> query,
        int? searchTopic,
        int pageNumber,
        int pageSize)
    {
        if (searchTopic != null)
        {
            query = query.Where(trainingList => trainingList.Topic == searchTopic);
        }

        var paginationResult = await query.PaginateAsync(pageNumber, pageSize);
        return new PagedList<TrainingList>(paginationResult.PaginatedItems, new PageItem(pageNumber, pageSize), paginationResult.TotalCount);
    }
}
