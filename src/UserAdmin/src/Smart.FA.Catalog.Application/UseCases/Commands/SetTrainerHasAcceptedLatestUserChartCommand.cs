using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Smart.FA.Catalog.Application.SeedWork;
using Smart.FA.Catalog.Core.Exceptions;
using Smart.FA.Catalog.Infrastructure.Extensions;
using Smart.FA.Catalog.Infrastructure.Persistence;

namespace Smart.FA.Catalog.Application.UseCases.Commands;

public class SetTrainerHasAcceptedLatestUserChartCommand : IRequestHandler<SetTrainerHasAcceptedLatestUserChartRequest, SetTrainerHasAcceptedLatestUserChartResponse>
{
    private readonly ILogger<SetTrainerHasAcceptedLatestUserChartCommand> _logger;
    private readonly CatalogContext _context;

    public SetTrainerHasAcceptedLatestUserChartCommand(ILogger<SetTrainerHasAcceptedLatestUserChartCommand> logger, CatalogContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<SetTrainerHasAcceptedLatestUserChartResponse> Handle(SetTrainerHasAcceptedLatestUserChartRequest request, CancellationToken cancellationToken)
    {
        SetTrainerHasAcceptedLatestUserChartResponse response = new();
        var latestUserChart = await _context.UserCharts.GetLatestOrDefault(cancellationToken);

        if (latestUserChart is null)
        {
            throw new UserChartException(Errors.UserChart.DontExist);
        }

        var trainer = await _context.Trainers.FirstOrDefaultAsync(trainer => trainer.Id == request.TrainerId, cancellationToken);

        if (trainer is null)
        {
            throw new TrainerException(Errors.Trainer.TrainerDoesntExist(request.TrainerId));
        }

        trainer.ApproveUserChart(latestUserChart);

        _logger.LogInformation("trainer {Name} has accepted user chart version {Version}", trainer.ToString(), latestUserChart.Version);

        await _context.SaveChangesAsync(cancellationToken);
        response.UserChartId = latestUserChart.Id;
        response.SetSuccess();

        return response;
    }
}

public class SetTrainerHasAcceptedLatestUserChartRequest : IRequest<SetTrainerHasAcceptedLatestUserChartResponse>
{
    public int TrainerId { get; set; }
}

public class SetTrainerHasAcceptedLatestUserChartResponse : ResponseBase
{
    public int UserChartId { get; set; }
}
