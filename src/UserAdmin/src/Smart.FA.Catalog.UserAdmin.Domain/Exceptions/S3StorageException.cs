using Smart.FA.Catalog.UserAdmin.Domain.SeedWork;

namespace Smart.FA.Catalog.UserAdmin.Domain.Exceptions;

public class S3StorageException: DomainException
{
    public S3StorageException(Error error) : base(error)
    {
    }
}
