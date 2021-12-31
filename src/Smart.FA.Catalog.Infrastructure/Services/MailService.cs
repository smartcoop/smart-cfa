using System.Net.Mail;
using Core.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services;

public class MailService : IMailService
{
    private readonly string _sender;
    private readonly string _server;
    private readonly ILogger _logger;

    public MailService(IOptions<MailOptions> mailOptions, ILogger logger)
    {
        _sender = mailOptions.Value.Sender;
        _server = mailOptions.Value.Server;
        _logger = logger;
    }

    public Task SendAsync(string body, string recipents, string? subject, CancellationToken cancellationToken)
    {
        MailMessage message = new(_sender, recipents);
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
            _logger.LogError(e.ToString());
            throw;
        }
        return Task.CompletedTask;
    }
}
