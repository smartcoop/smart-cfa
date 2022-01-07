namespace Core.Exceptions;

public class GuardClauseException: CommonAggregateException
{
    public GuardClauseException(string message) : base(CommonExceptionCode.GuardClauseException, message)
    {
    }
}
