using Smart.FA.Catalog.Core.SeedWork;

namespace Smart.FA.Catalog.Core.Exceptions;

public class UserChartException: DomainException
{
    public UserChartException(Error error) : base(error)
    {
    }
}
