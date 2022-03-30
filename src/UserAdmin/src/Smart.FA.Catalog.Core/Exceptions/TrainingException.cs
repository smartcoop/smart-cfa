using Smart.FA.Catalog.Core.SeedWork;

namespace Smart.FA.Catalog.Core.Exceptions;

public class TrainingException: DomainException
{
    public TrainingException(Error error) : base(error)
    {
    }
}
