using System.Linq.Expressions;
using Smart.FA.Catalog.Core.Domain.Enumerations;
using Smart.FA.Catalog.Core.SeedWork;
using Smart.FA.Catalog.Shared.Domain.Enumerations.Training;

namespace Smart.FA.Catalog.Core.Domain.Specifications;

public class ValidStatusSpecification : Specification<Training>
{
    int[] validStatus = {TrainingStatus.Validated.Id, TrainingStatus.WaitingForValidation.Id};

    public override Expression<Func<Training, bool>> ToExpression() =>
        training => validStatus.Contains(training.Status.Id);
}


public class RecentSpecification : Specification<Training>
{
    public override Expression<Func<Training, bool>> ToExpression() =>
        training => DateTime.Compare(DateTime.UtcNow.AddMonths(-1), training.LastModifiedAt) < 0;
}
