using Smart.FA.Catalog.UserAdmin.Domain.SeedWork;

namespace Smart.FA.Catalog.UserAdmin.Domain.Exceptions;

public class TrainingException: DomainException
{
    public TrainingException(Error error) : base(error)
    {
    }
}
