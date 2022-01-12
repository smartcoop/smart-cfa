using System.Diagnostics;
using Core.Exceptions;

namespace Core.SeedWork;

public static class Guard
{
    public static void Requires(Func<bool> predicate, string message)
    {
        if (predicate()) return;
        throw new GuardClauseException(message);
    }

    [Conditional("DEBUG")]
    public static void Ensures(Func<bool> predicate, string message)
    {
        Debug.Assert(predicate(), message);
    }

    public static void AgainstNull(object argumentValue, string argumentName)
    {
        if (argumentValue == null)
            throw new ArgumentNullException(argumentName);
    }
}
