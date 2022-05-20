using MediatR;
using Microsoft.EntityFrameworkCore;
using Smart.FA.Catalog.Core.Domain;
using Smart.FA.Catalog.Infrastructure.Extensions;
using Smart.FA.Catalog.Infrastructure.Persistence;
using Smart.FA.Catalog.Shared.Collections;

namespace Smart.FA.Catalog.Application.UseCases.Queries;

/// <summary>
/// Searches trainings with criteria such title, status, topics.
/// </summary>
public class GetTrainingsByCriteriaQuery : IRequest<PagedList<Training>>
{
    public string? TrainerName { get; set; }

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
        // Includes necessary navigation properties.
        var query = BaseQuery();

        // Filter by status.
        if (request.Status is not null)
        {
            query = query.Where(training => training.StatusType == request.Status);
        }

        // Filter by title.
        if (!string.IsNullOrWhiteSpace(request.Title))
        {
            query = query.Where(training => training.Details.Any(details => details.Title.Contains(request.Title)));
        }

        // Filter by topics.
        if (request.Topics?.Any() == true)
        {
            foreach (var requestTopic in request.Topics)
            {
                query = query.Where(training => training.Topics.Any(topic => topic.Topic == requestTopic));
            }
        }

        // Filter by assigned trainers' names.
        if (!string.IsNullOrWhiteSpace(request.TrainerName))
        {
            query = query.Where(training => training.TrainerAssignments.Any(assignment => assignment.Trainer.Name.FirstName.Contains(request.TrainerName) || assignment.Trainer.Name.LastName.Contains(request.TrainerName)));
        }

        // Finally apply pagination.
        var trainings = await query.PaginateAsync(new PageItem(request.PageNumber, request.PageSize), cancellationToken);

        return trainings;
    }

    private IQueryable<Training> BaseQuery =>
        _catalogContext.Trainings
            .Include(training => training.Topics)
            .Include(training => training.Details)
            .Include(training => training.TrainerAssignments)
            .ThenInclude(assignment => assignment.Trainer);
}
