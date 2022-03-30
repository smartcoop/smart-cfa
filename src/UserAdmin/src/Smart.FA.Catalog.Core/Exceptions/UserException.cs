using Smart.FA.Catalog.Core.SeedWork;

namespace Smart.FA.Catalog.Core.Exceptions;

public class UserException: DomainException
{
    public UserException(Error error) : base(error)
    {
    }
}
