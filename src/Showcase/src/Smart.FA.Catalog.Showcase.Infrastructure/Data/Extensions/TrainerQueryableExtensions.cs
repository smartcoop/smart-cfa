using Microsoft.EntityFrameworkCore;
using Smart.FA.Catalog.Showcase.Domain.Models;

namespace Smart.FA.Catalog.Showcase.Infrastructure.Data.Extensions;

public static class TrainerQueryableExtensions
{
    public static async Task<int> TrainerCountAsync(this IQueryable<TrainerDetails> query)
    {
        return await query.Select(trainer => trainer.Id).Distinct().CountAsync();
    }
}
