namespace Core.SeedWork;

public abstract class DomainException : Exception
{
    public string Code { get; set; }

    protected DomainException(string code, string? message) : base(message)
    {
        Code = code;
    }
}
