using Core.SeedWork;

namespace Core.Exceptions;

public class TrainingException: DomainException
{
    public TrainingException(Error error) : base(error)
    {
    }
}
