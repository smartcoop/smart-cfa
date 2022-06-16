using Microsoft.EntityFrameworkCore;
using Smart.FA.Catalog.Shared.Collections;
using Smart.FA.Catalog.Showcase.Domain.Models;

namespace Smart.FA.Catalog.Showcase.Infrastructure.Data.Extensions;

public static class TrainerQueryableExtensions
{
    public static async Task<int> TrainerCountAsync(this IQueryable<TrainerDetails> query)
    {
        return await query.Select(trainer => trainer.Id).Distinct().CountAsync();
    }

    public static async Task<PagedList<TrainerList>> SearchPaginatedTrainersAsync(this IQueryable<TrainerList> query, string? searchKeyWord, int pageNumber, int pageSize)
    {
        if (!string.IsNullOrEmpty(searchKeyWord))
        {
            query = query.Where(trainerList => trainerList.FirstName.Contains(searchKeyWord) || trainerList.LastName.Contains(searchKeyWord));
        }

        var paginationResult = await query.PaginateAsync(pageNumber, pageSize, randomIds: true);
        return new PagedList<TrainerList>(paginationResult.PaginatedItems, new PageItem(pageNumber, pageSize), paginationResult.TotalCount);
    }
}
