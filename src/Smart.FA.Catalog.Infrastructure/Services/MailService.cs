using System.Net.Mail;
using Core.Services;
using Infrastructure.Services.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services;

public class MailService : IMailService
{
    private readonly string _sender;
    private readonly string _server;
    private readonly ILogger<MailService> _logger;

    public MailService(IOptions<MailOptions> mailOptions, ILogger<MailService> logger)
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
             _logger.LogError("{Exception}", e.ToString());
            throw;
        }
        return Task.CompletedTask;
    }
}
