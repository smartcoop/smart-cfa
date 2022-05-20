using FluentEmail.Core;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Smart.FA.Catalog.Showcase.Domain.Exceptions;
using Smart.FA.Catalog.Showcase.Domain.Options;

namespace Smart.FA.Catalog.Showcase.Infrastructure.Mailing.Contact;

/// <inheritdoc />
public class InquiryEmailService : IInquiryEmailService
{
    private readonly ILogger<InquiryEmailService> _logger;
    private readonly IFluentEmail _fluentEmail;
    private readonly InquiriesOptions _inquiriesSettings;

    public string Template => "Smart.FA.Catalog.Showcase.Infrastructure.Mailing.Contact.InquiryEmailTemplate.cshtml";

    public InquiryEmailService(ILogger<InquiryEmailService> logger, IFluentEmail fluentEmail, IOptions<InquiriesOptions> inquiriesOptions)
    {
        _logger = logger;
        _fluentEmail = fluentEmail;
        _inquiriesSettings = inquiriesOptions.Value;
    }

    /// <inheritdoc />
    public async Task<bool> SendEmailAsync(InquirySendEmailRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _fluentEmail.To(_inquiriesSettings.DefaultEmail)
                .ReplyTo(request.Email)
                .SetFrom(request.Email, request.Name)
                .Subject($"Smart Learning: {request.Name} a envoyé une question à partir du formulaire du site")
                .UsingTemplateFromEmbedded(Template, request, typeof(InquiryEmailService).Assembly)
                .SendAsync(cancellationToken);

            if (!result.Successful)
            {
                throw new EmailSendException(string.Join(", ", result.ErrorMessages));
            }

            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while sending a mail from {email} with message {message}", request.Email, request.Message);

            return false;
        }
    }
}

/// <summary>
/// A service that sends to the Smart Learning teams visitors' general inquiries.
/// </summary>
public interface IInquiryEmailService
{
    /// <summary>
    /// Sends to Smart Learning team an email containing an inquiry of a visitor.
    /// </summary>
    /// <param name="request">Content of the inquiry.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task's result is a <see cref="bool" /> indicating if the process succeeded or not.</returns>
    Task<bool> SendEmailAsync(InquirySendEmailRequest request, CancellationToken cancellationToken = default);
}
