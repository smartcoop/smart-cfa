using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Smart.FA.Catalog.Infrastructure.Extensions;

public static class GlobalQueryExtensions
{
    /// <summary>
    /// Set arbitrary global filtering <see cref="query"/> on all entries of a specific Type <see cref="T"/>
    /// </summary>
    /// <param name="modelBuilder">ModelBuilder object</param>
    /// <param name="query">Query to filter systematically the entity</param>
    /// <typeparam name="T">The type to apply the query on</typeparam>
    public static void SetFilterQueryOnEntityDerivedFrom<T>(this ModelBuilder modelBuilder, Expression<Func<T, bool>> query)
    {
        foreach (var mutableEntityType in modelBuilder.Model.GetEntityTypes().Where(entity => entity.ClrType.IsAssignableTo(typeof(T))))
        {
            // At runtime the parameter type is expected to be `mutableEntityType.ClrType` but query's type is T (which could  be a type that is not concrete like in an interface).
            // So we need to revisit the expression tree to change the parameter type to the actual clr entity parameter type.
            var actualEntityTypeParameter = Expression.Parameter(mutableEntityType.ClrType);
            var body = ReplacingExpressionVisitor.Replace(query.Parameters.First(), actualEntityTypeParameter, query.Body);
            var lambdaExpression = Expression.Lambda(body, actualEntityTypeParameter);
            // Apply global query filter https://docs.microsoft.com/en-us/ef/core/querying/filters
            mutableEntityType.SetQueryFilter(lambdaExpression);
        }
    }
}
