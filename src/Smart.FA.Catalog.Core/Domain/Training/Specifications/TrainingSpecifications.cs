using System.Linq.Expressions;
using Core.Domain.Enumerations;
using Core.SeedWork;

namespace Core.Domain.Specifications;

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
