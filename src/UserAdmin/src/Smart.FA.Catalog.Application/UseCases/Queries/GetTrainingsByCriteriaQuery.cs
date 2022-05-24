using System.Globalization;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Smart.FA.Catalog.Core.Domain;
using Smart.FA.Catalog.Core.Domain.Dto;
using Smart.FA.Catalog.Infrastructure.Extensions;
using Smart.FA.Catalog.Infrastructure.Persistence;
using Smart.FA.Catalog.Shared.Collections;

namespace Smart.FA.Catalog.Application.UseCases.Queries;

/// <summary>
/// Searches trainings with criteria such title, status, topics.
/// </summary>
public class GetTrainingsByCriteriaQuery : IRequest<PagedList<Training>>
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
public class GetTrainingsByCriteriaQueryHandler : IRequestHandler<GetTrainingsByCriteriaQuery, PagedList<Training>>
{
    private readonly CatalogContext _catalogContext;

    public GetTrainingsByCriteriaQueryHandler(CatalogContext catalogContext)
    {
        _catalogContext = catalogContext;
    }

    public async Task<PagedList<Training>> Handle(GetTrainingsByCriteriaQuery request, CancellationToken cancellationToken)
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

        var trainings = await query.PaginateAsync(new PageItem(request.PageNumber, request.PageSize), cancellationToken);

        return trainings;
    }
}
