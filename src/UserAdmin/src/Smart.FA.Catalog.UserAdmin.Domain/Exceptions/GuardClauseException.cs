using Smart.FA.Catalog.UserAdmin.Domain.SeedWork;

namespace Smart.FA.Catalog.UserAdmin.Domain.Exceptions;

public class GuardClauseException: DomainException
{
    public GuardClauseException(Error error) : base(error)
    {
    }
}
