using Castle.Core.Internal;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Smart.FA.Catalog.Application.SeedWork;
using Smart.FA.Catalog.Core.Domain;
using Smart.FA.Catalog.Infrastructure.Extensions;
using Smart.FA.Catalog.Infrastructure.Persistence;
using Smart.FA.Catalog.Shared.Collections;

namespace Smart.FA.Catalog.Application.UseCases.Queries;

public class GetOtherTrainersListQuery : IRequestHandler<GetOtherTrainersListRequest, PagedList<Trainer>>
{
    private readonly CatalogContext _catalogContext;

    public GetOtherTrainersListQuery(CatalogContext catalogContext)
    {
        _catalogContext = catalogContext;
    }

    public async Task<PagedList<Trainer>> Handle(GetOtherTrainersListRequest request, CancellationToken cancellationToken)
    {
        var trainerQueryable = _catalogContext.Trainers
            .Include(trainer => trainer.SocialNetworks)
            .Include(trainer => trainer.Approvals)
            .Include(trainer => trainer.Assignments)
            .Where(trainer => trainer.Id != request.SelfTrainerId);

        if (!string.IsNullOrEmpty(request.TrainerName))
        {
            trainerQueryable = trainerQueryable.Where(trainer => trainer.Name.FirstName.Contains(request.TrainerName!) || trainer.Name.LastName.Contains(request.TrainerName!));
        }

        return await trainerQueryable.PaginateAsync(request.PageItem, cancellationToken);
    }
}

public class GetOtherTrainersListRequest : IRequest<PagedList<Trainer>>
{
    public int SelfTrainerId { get; set; }
    public string? TrainerName { get; set; }
    public PageItem PageItem { get; set; }
}
