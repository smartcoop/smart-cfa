using Smart.FA.Catalog.Core.Exceptions;

namespace Smart.FA.Catalog.Core.SeedWork;

public abstract class DomainException : Exception
{
    public Error Error { get; }

    protected DomainException(Error error) : base(error.Message)
    {
        Error = error;
    }

    public override string ToString()
    {
        return Error.Serialize();
    }
}
