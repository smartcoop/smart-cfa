using Smart.FA.Catalog.Core.SeedWork;

namespace Smart.FA.Catalog.Core.Exceptions;

public class GuardClauseException: DomainException
{
    public GuardClauseException(Error error) : base(error)
    {
    }
}
