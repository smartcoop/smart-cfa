using Core.SeedWork;

namespace Core.Exceptions;

public class CommonExceptionCode: Enumeration
{
    public static readonly CommonExceptionCode GuardClauseException = new CommonExceptionCode(1, "Guard Clause Exception:");
    public CommonExceptionCode(int id, string name): base(id, name)
    { }

}
