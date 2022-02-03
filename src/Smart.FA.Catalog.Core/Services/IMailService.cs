namespace Core.Services;

public interface IMailService
{
    public Task SendAsync(string body, string receipents, string? subject, CancellationToken cancellationToken);
}
