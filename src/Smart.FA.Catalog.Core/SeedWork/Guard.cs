using System.Diagnostics;
using Smart.FA.Catalog.Core.Exceptions;
using Smart.FA.Catalog.Core.Helpers;

namespace Smart.FA.Catalog.Core.SeedWork;

public static class Guard
{
    public static void Requires(Func<bool> predicate, string message)
    {
        if (predicate()) return;
        throw new GuardClauseException(Errors.General.GuardClauseBlockage(message));
    }

    [Conditional("DEBUG")]
    public static void Ensures(Func<bool> predicate, string message)
    {
        Debug.Assert(predicate(), message);
    }

    public static void AgainstNull(object? argumentValue, string argumentName)
    {
        if (argumentValue is null)
            throw new ArgumentNullException(argumentName);
    }

    public static string AgainstInvalidEmail(string? email, string parameterName, string? message = null)
    {
        AgainstNull(email, parameterName);
        if (!EmailValidator.Validate(email!))
        {
            throw new ArgumentException(message ?? $"{email} is an invalid email", parameterName);
        }

        return email!;
    }
}
