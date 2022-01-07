using Core.SeedWork;

namespace Core.Exceptions;

public class CommonAggregateException: DomainException
{
    public CommonAggregateException( CommonExceptionCode code , string message = null ) : base( code.ToString(), message ) { }
}

