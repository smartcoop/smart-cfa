namespace Smart.FA.Catalog.Showcase.Domain.Exceptions;

public class EmailSendException : Exception
{
    public EmailSendException(string errors) : base(errors)
    {
    }
}