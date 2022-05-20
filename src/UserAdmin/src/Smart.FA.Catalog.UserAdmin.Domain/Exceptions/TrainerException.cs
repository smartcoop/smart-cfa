using Smart.FA.Catalog.UserAdmin.Domain.SeedWork;

namespace Smart.FA.Catalog.UserAdmin.Domain.Exceptions;

public class TrainerException: DomainException
{
    public TrainerException(Error error) : base(error)
    {
    }
}
