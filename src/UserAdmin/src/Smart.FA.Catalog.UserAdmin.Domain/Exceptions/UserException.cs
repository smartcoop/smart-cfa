using Smart.FA.Catalog.UserAdmin.Domain.SeedWork;

namespace Smart.FA.Catalog.UserAdmin.Domain.Exceptions;

public class UserException: DomainException
{
    public UserException(Error error) : base(error)
    {
    }
}
