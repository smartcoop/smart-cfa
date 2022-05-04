using System.Globalization;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Smart.FA.Catalog.Core.Domain;
using Smart.FA.Catalog.Core.Domain.Dto;
using Smart.FA.Catalog.Infrastructure.Persistence;

namespace Smart.FA.Catalog.Application.UseCases.Queries;

/// <summary>
/// Searches trainings with criteria such title, status, topics.
/// </summary>
public class GetTrainingsByCriteriaQuery : IRequest<PaginatedList<TrainingDto>>
{
    public string? Title { get; init; }

    public int? Status { get; set; }

    public List<int>? Topics { get; set; }

    public int PageSize { get; set; } = 20;

    public int PageNumber { get; set; } = 1;
}

/// <summary>
/// Handles <see cref="GetTrainingsByCriteriaQuery" />.
/// </summary>
public class GetTrainingsByCriteriaQueryHandler : IRequestHandler<GetTrainingsByCriteriaQuery, PaginatedList<TrainingDto>>
{
    private readonly CatalogContext _catalogContext;

    public GetTrainingsByCriteriaQueryHandler(CatalogContext catalogContext)
    {
        _catalogContext = catalogContext;
    }

    public async Task<PaginatedList<TrainingDto>> Handle(GetTrainingsByCriteriaQuery request, CancellationToken cancellationToken)
    {
        IQueryable<Training> query = _catalogContext.Trainings
            .Include(training => training.Topics)
            .Include(training => training.Details);

        if (request.Status is not null)
        {
            query = query.Where(training => training.StatusType == request.Status);
        }

        if (!string.IsNullOrWhiteSpace(request.Title))
        {
            query = query.Where(training => training.Details.Any(details => details.Title.Contains(request.Title)));
        }

        if (request.Topics?.Any() == true)
        {
            foreach (var requestTopic in request.Topics)
            {
                query = query.Where(training => training.Topics.Any(topic => topic.Topic == requestTopic));
            }
        }

        var trainings = await query.PaginateAsync(request.PageNumber, request.PageSize, cancellationToken);

        return new PaginatedList<TrainingDto>(MapTrainingsToDtos(trainings), trainings.TotalCount, request.PageNumber, request.PageSize);
    }

    private static List<TrainingDto> MapTrainingsToDtos(PaginatedList<Training> trainings)
    {
        return trainings.Select(training =>
        {
            // We don't know if there is a details for the underlying selected locale, so we return the existing one if not.
            var eurrentCultureAlpha2Code = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            var detail = training.Details.FirstOrDefault(detail => string.Equals(detail.Language.Value, eurrentCultureAlpha2Code, StringComparison.OrdinalIgnoreCase))
                         ?? training.Details.First();

            return new TrainingDto(training.Id,
                training.StatusType.Id,
                detail.Title,
                detail.Goal,
                detail.Language.Value,
                training.Topics.Select(topic => topic.Topic.Id).ToList());
        }).ToList();
    }
}

public class PaginatedList<T> : List<T>
{
    public int PageIndex { get; }

    public int PageSize { get; }

    public int TotalCount { get; }

    public int TotalPages { get; }

    public PaginatedList(List<T> items, int count, int pageIndex, int pageSize) : base(items)
    {
        PageIndex  = pageIndex;
        PageSize   = pageSize;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        TotalCount = count;
    }

    public bool HasPreviousPage => PageIndex > 1;

    public bool HasNextPage => PageIndex < TotalPages;

    public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize, CancellationToken cancellationToken)
    {
        var count = await source.CountAsync(cancellationToken);
        var skip = Math.Max((pageIndex - 1) * pageSize, 0);
        var take = Math.Max(pageSize, 0);

        var items = await source.Skip(skip).Take(take).ToListAsync(cancellationToken);
        return new PaginatedList<T>(items, count, pageIndex, pageSize);
    }
}

public static class PaginationExtensions
{
    /// <summary>
    /// Applies pagination to an <see cref="IQueryable{T}" />.
    /// </summary>
    /// <typeparam name="T">The type of the data in the data source.</typeparam>
    /// <param name="query">Source query on which the pagination will be applied.</param>
    /// <param name="pageIndex">Current page. PageIndex start at one.</param>
    /// <param name="pageSize">Number of elements per page.</param>
    /// <param name="cancellationToken">A token to cancel the operations.</param>
    /// <returns>A task that represents the asynchronous operation. The task's result is a <see cref="PaginatedList{T}" /> on which was apply pagination.</returns>
    public static async Task<PaginatedList<T>> PaginateAsync<T>(this IQueryable<T> query, int pageIndex, int pageSize, CancellationToken cancellationToken)
    {
        return await PaginatedList<T>.CreateAsync(query, pageIndex, pageSize, cancellationToken);
    }
}
