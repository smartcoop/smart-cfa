using MediatR;
using Microsoft.Extensions.Logging;
using Smart.FA.Catalog.Application.SeedWork;
using Smart.FA.Catalog.Core.Domain;
using Smart.FA.Catalog.Infrastructure.Persistence;

namespace Smart.FA.Catalog.Application.UseCases.Queries;

public class GetTrainingByIdQueryHandler: IRequestHandler<GetTrainingByIdRequest, GetTrainingByIdResponse>
{
    private readonly ILogger<GetTrainingByIdQueryHandler> _logger;
    private readonly CatalogContext _catalogContext;

    public GetTrainingByIdQueryHandler(ILogger<GetTrainingByIdQueryHandler> logger, CatalogContext catalogContext )
    {
        _logger = logger;
        _catalogContext = catalogContext;
    }

    public async Task<GetTrainingByIdResponse> Handle(GetTrainingByIdRequest request, CancellationToken cancellationToken)
    {
        GetTrainingByIdResponse resp = new();
        var training = await _catalogContext.Trainings.FindAsync(new object?[] { request.TrainingId }, cancellationToken: cancellationToken);
        resp.Training = training!;
        resp.SetSuccess();

        return resp;
    }
}

public class GetTrainingByIdRequest: IRequest<GetTrainingByIdResponse>
{
    public int TrainingId { get; set; }
}

public class GetTrainingByIdResponse : ResponseBase
{
    public Training? Training { get; set; }
}
