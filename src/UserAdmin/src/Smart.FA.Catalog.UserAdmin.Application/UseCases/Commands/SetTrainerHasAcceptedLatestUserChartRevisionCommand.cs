using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Smart.FA.Catalog.UserAdmin.Application.SeedWork;
using Smart.FA.Catalog.UserAdmin.Domain.Exceptions;
using Smart.FA.Catalog.UserAdmin.Infrastructure.Extensions;
using Smart.FA.Catalog.UserAdmin.Infrastructure.Persistence;

namespace Smart.FA.Catalog.UserAdmin.Application.UseCases.Commands;

public class SetTrainerHasAcceptedLatestUserChartRevisionCommand : IRequestHandler<SetTrainerHasAcceptedLatestUserChartRequest, SetTrainerHasAcceptedLatestUserChartRevisionResponse>
{
    private readonly ILogger<SetTrainerHasAcceptedLatestUserChartRevisionCommand> _logger;
    private readonly CatalogContext _context;

    public SetTrainerHasAcceptedLatestUserChartRevisionCommand(ILogger<SetTrainerHasAcceptedLatestUserChartRevisionCommand> logger, CatalogContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<SetTrainerHasAcceptedLatestUserChartRevisionResponse> Handle(SetTrainerHasAcceptedLatestUserChartRequest request, CancellationToken cancellationToken)
    {
        SetTrainerHasAcceptedLatestUserChartRevisionResponse response = new();
        var latestUserChartRevision = await _context.UserChartRevisions.GetLatestCreatedOrDefaultAsync(cancellationToken);

        if (latestUserChartRevision is null)
        {
            throw new UserChartRevisionException(Errors.UserChartRevision.DontExist);
        }

        var trainer = await _context.Trainers
            .Include(trainer => trainer.Approvals)
            .FirstOrDefaultAsync(trainer => trainer.Id == request.TrainerId, cancellationToken);

        if (trainer is null)
        {
            throw new TrainerException(Errors.Trainer.TrainerDoesntExist(request.TrainerId));
        }

        trainer.ApproveUserChart(latestUserChartRevision);

        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("trainer {Id} has accepted user chart version {Version}", trainer.Id, latestUserChartRevision.Version);

        response.UserChartRevisionId = latestUserChartRevision.Id;
        response.SetSuccess();

        return response;
    }
}

public class SetTrainerHasAcceptedLatestUserChartRequest : IRequest<SetTrainerHasAcceptedLatestUserChartRevisionResponse>
{
    public int TrainerId { get; set; }
}

public class SetTrainerHasAcceptedLatestUserChartRevisionResponse : ResponseBase
{
    public int UserChartRevisionId { get; set; }
}
