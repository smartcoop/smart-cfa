using Smart.FA.Catalog.Core.SeedWork;

namespace Smart.FA.Catalog.Core.Exceptions;

public class S3StorageException: DomainException
{
    public S3StorageException(Error error) : base(error)
    {
    }
}
