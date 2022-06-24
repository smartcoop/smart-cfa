using MediatR;
using Microsoft.EntityFrameworkCore;
using Smart.FA.Catalog.Application.SeedWork;
using Smart.FA.Catalog.Infrastructure.Persistence;

namespace Smart.FA.Catalog.Application.UseCases.Commands;

public class DeleteTrainerCommand : IRequestHandler<DeleteTrainerRequest, DeleteTrainerResponse>
{
    private readonly CatalogContext _catalogContext;

    public DeleteTrainerCommand(CatalogContext catalogContext)
    {
        _catalogContext = catalogContext;
    }

    public async Task<DeleteTrainerResponse> Handle(DeleteTrainerRequest request, CancellationToken cancellationToken)
    {
        DeleteTrainerResponse response = new();
        var trainer = await _catalogContext.Trainers.FirstOrDefaultAsync(trainer => trainer.Id == request.TrainerId, cancellationToken);
        _catalogContext.Remove(trainer);
        await _catalogContext.SaveChangesAsync(cancellationToken);
        response.SetSuccess();
        return response;
    }
}

public class DeleteTrainerRequest : IRequest<DeleteTrainerResponse>
{
    public int TrainerId { get; set; }
}

public class DeleteTrainerResponse : ResponseBase
{
}
