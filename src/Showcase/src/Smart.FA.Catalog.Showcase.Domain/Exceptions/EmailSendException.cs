namespace Smart.FA.Catalog.Showcase.Domain.Exceptions;

public class EmailSendException : Exception
{
    public EmailSendException(string? message) : base(message)
    {
    }

    public EmailSendException(string errorMessage, Exception innerException) : base(errorMessage, innerException)
    {
    }
}
