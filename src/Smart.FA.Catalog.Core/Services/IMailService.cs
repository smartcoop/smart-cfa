namespace Core.Services;

public interface IMailService
{
    public Task SendAsync(string body, string recipents, string? subject, CancellationToken cancellationToken);
}
