using System.Net.Mail;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Smart.FA.Catalog.UserAdmin.Domain.Services;
using Smart.FA.Catalog.UserAdmin.Infrastructure.Services.Options;

namespace Smart.FA.Catalog.UserAdmin.Infrastructure.Services;

public class MailService : IMailService
{
    private readonly string _sender;
    private readonly string _server;
    private readonly ILogger<MailService> _logger;

    public MailService(IOptions<MailOptions> mailOptions, ILogger<MailService> logger)
    {
        _sender = mailOptions.Value.Sender ?? null!;
        _server = mailOptions.Value.Server ?? null!;
        _logger = logger;
    }

    public Task SendAsync(string body, string receipents, string? subject, CancellationToken cancellationToken)
    {
        MailMessage message = new(_sender, receipents);
        message.Body = body;
        message.Subject = subject;

        SmtpClient client = new(_server);
        client.UseDefaultCredentials = true;
        try
        {
            client.SendAsync(message, cancellationToken);
        }
        catch (Exception e)
        {
             _logger.LogError("{Exception}", e.ToString());
            throw;
        }
        return Task.CompletedTask;
    }
}
