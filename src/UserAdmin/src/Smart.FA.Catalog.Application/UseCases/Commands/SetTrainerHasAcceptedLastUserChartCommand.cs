using MediatR;
using Microsoft.EntityFrameworkCore;
using Smart.FA.Catalog.Application.SeedWork;
using Smart.FA.Catalog.Infrastructure.Persistence;

namespace Smart.FA.Catalog.Application.UseCases.Commands;

public class SetTrainerHasAcceptedLastUserChartCommand : IRequestHandler<SetTrainerHasAcceptedLastUserChartRequest, SetTrainerHasAcceptedLastUserChartResponse>
{
    private readonly CatalogContext _context;

    public SetTrainerHasAcceptedLastUserChartCommand(CatalogContext context)
    {
        _context = context;
    }

    public async Task<SetTrainerHasAcceptedLastUserChartResponse> Handle(SetTrainerHasAcceptedLastUserChartRequest request, CancellationToken cancellationToken)
    {
        SetTrainerHasAcceptedLastUserChartResponse response = new();
        var lastUserChart = await _context.UserCharts.OrderByDescending(userChart => userChart.CreatedAt).FirstOrDefaultAsync(cancellationToken);
        var trainer = await _context.Trainers.FirstOrDefaultAsync(trainer => trainer.Id == request.TrainerId, cancellationToken);

        trainer!.ApproveUserChart(lastUserChart);
        await _context.SaveChangesAsync(cancellationToken);
        response.UserChartId = lastUserChart!.Id;
        response.SetSuccess();

        return response;
    }
}

public class SetTrainerHasAcceptedLastUserChartRequest : IRequest<SetTrainerHasAcceptedLastUserChartResponse>
{
    public int TrainerId { get; set; }
}

public class SetTrainerHasAcceptedLastUserChartResponse : ResponseBase
{
    public int UserChartId { get; set; }
}
