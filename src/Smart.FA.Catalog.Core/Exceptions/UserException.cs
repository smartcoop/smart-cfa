using Core.SeedWork;

namespace Core.Exceptions;

public class UserException: DomainException
{
    public UserException(Error error) : base(error)
    {
    }
}
