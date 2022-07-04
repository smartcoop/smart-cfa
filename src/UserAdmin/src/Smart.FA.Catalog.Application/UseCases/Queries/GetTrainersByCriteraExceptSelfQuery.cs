using MediatR;
using Microsoft.EntityFrameworkCore;
using Smart.FA.Catalog.Core.Domain;
using Smart.FA.Catalog.Infrastructure.Extensions;
using Smart.FA.Catalog.Infrastructure.Persistence;
using Smart.FA.Catalog.Shared.Collections;

namespace Smart.FA.Catalog.Application.UseCases.Queries;

public class GetTrainersByCriteriaExceptSelfQuery : IRequestHandler<GetTrainersByCriteriaExceptSelfListRequest, PagedList<Trainer>>
{
    private readonly CatalogContext _catalogContext;

    public GetTrainersByCriteriaExceptSelfQuery(CatalogContext catalogContext)
    {
        _catalogContext = catalogContext;
    }

    public async Task<PagedList<Trainer>> Handle(GetTrainersByCriteriaExceptSelfListRequest request, CancellationToken cancellationToken)
    {
        if (request.SelfTrainerId == default)
        {
            throw new ArgumentException("Connected trainer id cannot be 0");
        }

        var trainerQueryable = _catalogContext.Trainers
            .Include(trainer => trainer.SocialNetworks)
            .Include(trainer => trainer.Approvals)
            .Include(trainer => trainer.Assignments)
            .Where(trainer => trainer.Id != request.SelfTrainerId);

        if (!string.IsNullOrEmpty(request.TrainerNameOrEmailQueryFilter))
        {
            trainerQueryable = trainerQueryable
                .Where(trainer => trainer.Name.FirstName.Contains(request.TrainerNameOrEmailQueryFilter!)
                    || trainer.Name.LastName.Contains(request.TrainerNameOrEmailQueryFilter!)
                    || trainer.Email!.Contains(request.TrainerNameOrEmailQueryFilter));
        }

        return await trainerQueryable.PaginateAsync(request.PageItem, cancellationToken);
    }
}

public class GetTrainersByCriteriaExceptSelfListRequest : IRequest<PagedList<Trainer>>
{
    public int SelfTrainerId { get; set; }
    public string? TrainerNameOrEmailQueryFilter { get; set; }
    public PageItem PageItem { get; set; } = null!;
}
