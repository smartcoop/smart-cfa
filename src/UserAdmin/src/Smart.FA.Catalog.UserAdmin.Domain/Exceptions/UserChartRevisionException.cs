using Smart.FA.Catalog.UserAdmin.Domain.SeedWork;

namespace Smart.FA.Catalog.UserAdmin.Domain.Exceptions;

public class UserChartRevisionException: DomainException
{
    public UserChartRevisionException(Error error) : base(error)
    {
    }
}
