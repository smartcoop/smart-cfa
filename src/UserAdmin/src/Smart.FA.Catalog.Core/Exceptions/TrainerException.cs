using Smart.FA.Catalog.Core.SeedWork;

namespace Smart.FA.Catalog.Core.Exceptions;

public class TrainerException: DomainException
{
    public TrainerException(Error error) : base(error)
    {
    }
}
