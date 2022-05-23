using FluentEmail.Core;
using FluentEmail.Core.Models;
using Hangfire;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Smart.FA.Catalog.Showcase.Domain.Common.Enums;
using Smart.FA.Catalog.Showcase.Domain.Common.Exceptions;
using Smart.FA.Catalog.Showcase.Domain.Common.Options;

namespace Smart.FA.Catalog.Showcase.Infrastructure.Mailing.Contact;

/// <inheritdoc />
public class InquiryEmailService : IInquiryEmailService
{
    private readonly ILogger<InquiryEmailService> _logger;
    private readonly IFluentEmail _fluentEmail;
    private readonly IMemoryCache _memoryCache;
    private readonly IBackgroundJobClient _backgroundJobClient;
    private readonly InquiriesOptions _inquiriesSettings;

    public string Template => "Smart.FA.Catalog.Showcase.Infrastructure.Mailing.Contact.InquiryEmailTemplate.cshtml";

    public InquiryEmailService(ILogger<InquiryEmailService> logger,
        IFluentEmail fluentEmail,
        IMemoryCache memoryCache,
        IBackgroundJobClient backgroundJobClient,
        IOptions<InquiriesOptions> inquiriesOptions)
    {
        _logger = logger;
        _fluentEmail = fluentEmail;
        _backgroundJobClient = backgroundJobClient;
        _memoryCache = memoryCache;
        _inquiriesSettings = inquiriesOptions.Value;
    }

    /// <inheritdoc />
    public InquirySendEmailResult SendEmail(InquirySendEmailRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            EnsureRateLimit(request.RemoteIpAddress);
            _backgroundJobClient.Enqueue(() => SendEmailInternalAsync(request, cancellationToken));
            return InquirySendEmailResult.Ok;
        }
        catch (RateLimitException)
        {
            return InquirySendEmailResult.TooManyRequest;
        }
        catch (Exception ex)
        {
            LogError(request, ex);
            return InquirySendEmailResult.Failure;
        }
    }

    private static readonly SemaphoreSlim EmailSendSemaphore = new(1,1);

    /// <summary>
    /// Sends to Smart Learning team an email containing an inquiry of a visitor.
    /// </summary>
    /// <param name="request">Content of the inquiry.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <return>A <see cref="Task" /> that represents the asynchronous operation.</return>
    /// <exception cref="EmailSendException">If any failure happens.</exception>
    public async Task SendEmailInternalAsync(InquirySendEmailRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            // Prevent concurrent executions.
            await EmailSendSemaphore.WaitAsync(cancellationToken);

            // Will throw an exception if an email was already sent in the allowed time frame.
            EnsureRateLimit(request.RemoteIpAddress);

            var response = await SendWithFluentEmailAsync(request, cancellationToken);

            EnsureEmailSendingResponse(response);
            RegisterSendTimeForRateLimit(request.RemoteIpAddress);
        }
        catch (Exception e) when (e is not RateLimitException)
        {
            var errorMessage = LogError(request, e);

            // This makes sure that Hangfire will retry upon an occurring error.
            throw new EmailSendException(errorMessage, e);
        }
        finally
        {
            EmailSendSemaphore.Release();
        }
    }

    private async Task<SendResponse> SendWithFluentEmailAsync(InquirySendEmailRequest request, CancellationToken cancellationToken)
    {
        var result = await _fluentEmail.To(_inquiriesSettings.DefaultEmail)
            .ReplyTo(request.Email)
            .SetFrom(request.Email, request.Name)
            .Subject($"Smart Learning: {request.Name} a envoyé une question à partir du formulaire du site")
            .UsingTemplateFromEmbedded(Template, request, typeof(InquiryEmailService).Assembly)
            .SendAsync(cancellationToken);

        return result;
    }

    /// <summary>
    /// Ensures that <paramref name="remoteIpAddress" /> is not sending a mail within <see cref="InquiriesOptions.RateLimitInSeconds" /> seconds.
    /// </summary>
    /// <param name="remoteIpAddress">The remote IP address of the sender.</param>
    /// <exception cref="RateLimitException">The IP address has already sent a inquiry within the allowed time frame.</exception>
    private void EnsureRateLimit(string remoteIpAddress)
    {
        var lastTimeSentEmail = _memoryCache.Get<DateTime?>(GetKey(remoteIpAddress));

        if (lastTimeSentEmail is not null)
        {
            throw new RateLimitException(remoteIpAddress);
        }
    }

    private void EnsureEmailSendingResponse(SendResponse result)
    {
        if (!result.Successful)
        {
            throw new Exception(string.Join(", ", result.ErrorMessages));
        }
    }

    private void RegisterSendTimeForRateLimit(string ipAddress)
    {
        _memoryCache.Set(GetKey(ipAddress), DateTime.UtcNow, TimeSpan.FromSeconds(_inquiriesSettings.RateLimitInSeconds));
    }

    private string LogError(InquirySendEmailRequest request, Exception e)
    {
        var errorMessage = $"An error occurred while sending a mail from {request.Email} with message {request.Message}";
        _logger.LogError(e, errorMessage);
        return errorMessage;
    }

    private string GetKey(string ipAddress)
    {
        return "inquiry-rate-limit-" + ipAddress;
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
    /// This method is safe, i.e., it captures any encountered exceptions.
    /// </summary>
    /// <param name="request">Content of the inquiry.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    InquirySendEmailResult SendEmail(InquirySendEmailRequest request, CancellationToken cancellationToken = default);
}
