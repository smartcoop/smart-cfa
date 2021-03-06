using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Smart.FA.Catalog.Application.SeedWork;
using Smart.FA.Catalog.Core.Domain;
using Smart.FA.Catalog.Core.Domain.ValueObjects;
using Smart.FA.Catalog.Infrastructure.Persistence;
using Smart.FA.Catalog.Infrastructure.Extensions;
using Smart.FA.Catalog.Shared.Collections;

namespace Smart.FA.Catalog.Application.UseCases.Queries;

public class GetPagedTrainingListFromTrainerQueryHandler : IRequestHandler<GetPagedTrainingListFromTrainerRequest, GetPagedTrainingListFromTrainerResponse>
{
    private readonly ILogger<GetPagedTrainingListFromTrainerQueryHandler> _logger;
    private readonly CatalogContext _context;

    public GetPagedTrainingListFromTrainerQueryHandler(ILogger<GetPagedTrainingListFromTrainerQueryHandler> logger, CatalogContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<GetPagedTrainingListFromTrainerResponse> Handle(GetPagedTrainingListFromTrainerRequest request,
        CancellationToken cancellationToken)
    {
        GetPagedTrainingListFromTrainerResponse resp = new();

        var trainingsFromTrainerQueryable = _context
            .TrainerAssignments
            .AsNoTracking()
            .Include(assignment => assignment.Training.Details)
            .Include(assignment => assignment.Training.Topics)
            .Where(assignment => assignment.Trainer.Id == request.TrainerId)
            .Select(assignment => assignment.Training)
            .Where(training => training.Details.Any(details => details.Language == request.Language))
            .OrderByDescending(training => training.LastModifiedAt);

        var trainings = await trainingsFromTrainerQueryable.PaginateAsync(request.PageItem, cancellationToken);

        resp.Trainings = trainings;
        resp.SetSuccess();

        return resp;
    }
}

public class GetPagedTrainingListFromTrainerRequest : IRequest<GetPagedTrainingListFromTrainerResponse>
{
    public int TrainerId { get; init; }
    public Language Language { get; init; } = null!;
    public PageItem PageItem { get; init; } = null!;
}

public class GetPagedTrainingListFromTrainerResponse : ResponseBase
{
    public PagedList<Training>? Trainings { get; set; }
}
