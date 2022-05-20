namespace Smart.FA.Catalog.UserAdmin.Domain.Services;

public interface IMailService
{
    public Task SendAsync(string body, string receipents, string? subject, CancellationToken cancellationToken);
}
