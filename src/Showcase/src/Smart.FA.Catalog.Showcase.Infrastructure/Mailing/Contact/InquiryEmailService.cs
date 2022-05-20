using FluentEmail.Core;
using Hangfire;
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
    private readonly IBackgroundJobClient _backgroundJobClient;
    private readonly InquiriesOptions _inquiriesSettings;

    public string Template => "Smart.FA.Catalog.Showcase.Infrastructure.Mailing.Contact.InquiryEmailTemplate.cshtml";

    public InquiryEmailService(ILogger<InquiryEmailService> logger, IFluentEmail fluentEmail, IBackgroundJobClient backgroundJobClient,  IOptions<InquiriesOptions> inquiriesOptions)
    {
        _logger = logger;
        _fluentEmail = fluentEmail;
        _backgroundJobClient = backgroundJobClient;
        _inquiriesSettings = inquiriesOptions.Value;
    }

    /// <inheritdoc />
    public void SendEmail(InquirySendEmailRequest request, CancellationToken cancellationToken = default)
    {
        _backgroundJobClient.Enqueue(() => SendEmailAsyncInternal(request, cancellationToken));
    }

    /// <summary>
    /// Sends to Smart Learning team an email containing an inquiry of a visitor.
    /// </summary>
    /// <param name="request">Content of the inquiry.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <exception cref="EmailSendException">If any failure happens.</exception>
    public async Task<bool> SendEmailAsyncInternal(InquirySendEmailRequest request, CancellationToken cancellationToken = default)
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
                throw new Exception(string.Join(", ", result.ErrorMessages));
            }

            return true;
        }
        catch (Exception e)
        {
            var errorMessage = $"An error occurred while sending a mail from {request.Email} with message {request.Message}";
            _logger.LogError(e, errorMessage);

            // This makes sure that Hangfire will retry upon an occurring error.
            throw new EmailSendException(errorMessage, e);
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
    /// The execution of the code is handled by Hangfire.
    /// </summary>
    /// <param name="request">Content of the inquiry.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    void SendEmail(InquirySendEmailRequest request, CancellationToken cancellationToken = default);
}
