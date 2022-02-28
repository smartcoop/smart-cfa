using Core.SeedWork;

namespace Core.Exceptions;

public class GuardClauseException: DomainException
{
    public GuardClauseException(Error error) : base(error)
    {
    }
}
