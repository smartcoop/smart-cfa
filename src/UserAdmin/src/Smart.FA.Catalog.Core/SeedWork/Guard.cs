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

    public static string? AgainstMaxLength(string? input, string parameterName, int maxValue, string? message = null)
    {
        if (input is not null && input.Length > maxValue)
        {
            throw new ArgumentException(message ?? $"{parameterName} needs a maximum length of {maxValue} characters");
        }

        return input;
    }

    public static string? AgainstMinLength(string? input, string parameterName, int minValue, string? message = null)
    {
        if (input is not null && input.Length < minValue)
        {
            throw new ArgumentException(message ?? $"{parameterName} needs a minimum length of {minValue} characters");
        }

        return input;
    }

    public static string? AgainstInBetweenLength(string? input, string parameterName, int minValue, int maxValue, string? message = null)
    {
        if (input is not null && input.Length < minValue && input.Length > maxValue)
        {
            throw new ArgumentException(message ?? $"{parameterName} needs a minimum length of {minValue} characters");
        }

        return input;
    }
}
