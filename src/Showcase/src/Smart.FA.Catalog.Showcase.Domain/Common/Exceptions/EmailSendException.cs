namespace Smart.FA.Catalog.Showcase.Domain.Common.Exceptions;

public class EmailSendException : Exception
{
    public EmailSendException(string errorMessage, Exception innerException) : base(errorMessage, innerException)
    {
    }
}
