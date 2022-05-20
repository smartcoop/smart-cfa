using Smart.FA.Catalog.UserAdmin.Domain.Exceptions;

namespace Smart.FA.Catalog.UserAdmin.Domain.SeedWork;

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
