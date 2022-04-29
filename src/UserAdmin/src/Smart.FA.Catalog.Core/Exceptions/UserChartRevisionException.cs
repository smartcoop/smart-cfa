using Smart.FA.Catalog.Core.SeedWork;

namespace Smart.FA.Catalog.Core.Exceptions;

public class UserChartRevisionException: DomainException
{
    public UserChartRevisionException(Error error) : base(error)
    {
    }
}
