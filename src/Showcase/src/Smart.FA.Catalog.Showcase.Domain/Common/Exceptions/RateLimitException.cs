namespace Smart.FA.Catalog.Showcase.Domain.Common.Exceptions;

public class RateLimitException : Exception
{
    public RateLimitException(string? message) : base(message)
    {
    }
}
