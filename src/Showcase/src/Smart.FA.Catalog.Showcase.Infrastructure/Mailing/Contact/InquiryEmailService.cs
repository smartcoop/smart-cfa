using FluentEmail.Core;
using FluentEmail.Core.Models;
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
    //private readonly IBackgroundJobClient _backgroundJobClient;
    private readonly FluentEmailOptions _fluentEmailSettings;
    private readonly InquiryOptions _inquiriesSettings;

    public string Template => "Smart.FA.Catalog.Showcase.Infrastructure.Mailing.Contact.InquiryEmailTemplate.cshtml";

    public InquiryEmailService(ILogger<InquiryEmailService> logger,
        IFluentEmail fluentEmail,
        IMemoryCache memoryCache,
        /*IBackgroundJobClient backgroundJobClient,*/
        IOptions<FluentEmailOptions> fluentEmailOptions,
        IOptions<InquiryOptions> inquiriesOptions)
    {
        _logger = logger;
        _fluentEmail = fluentEmail;
        //_backgroundJobClient = backgroundJobClient;
        _memoryCache = memoryCache;
        _fluentEmailSettings = fluentEmailOptions.Value;
        _inquiriesSettings = inquiriesOptions.Value;
    }

    /// <inheritdoc />
    public InquirySendEmailResult SendEmail(InquirySendEmailRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            EnsureRateLimit(request.RemoteIpAddress);
            Task.Run(async () => await SendWithRetriesAsync(request, cancellationToken), cancellationToken);
            return InquirySendEmailResult.Ok;
        }
        catch (RateLimitException)
        {
            return InquirySendEmailResult.TooManyRequests;
        }
    }

    private async Task SendWithRetriesAsync(InquirySendEmailRequest request, CancellationToken cancellationToken = default)
    {
        // 11, 12, 17, 29, 56, 117, 252, 550 seconds between retries.
        int DelayToWaitBetweenRetriesInMilliseconds(int retryAttempt) => (int)(Math.Max(10 - retryAttempt, 0) + Math.Pow(2.2, Math.Min(retryAttempt, 40))) * 1_000;
        for (var i = 1; i < 9; i++)
        {
            try
            {
                await SendEmailInternalAsync(request, cancellationToken);

                break;
            }
            catch (Exception)
            {
                await Task.Delay(DelayToWaitBetweenRetriesInMilliseconds(i), cancellationToken);
            }
        }
    }

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
            // Will throw an exception if an email was already sent in the allowed time frame.
            //EnsureRateLimit(request.RemoteIpAddress);

            var response = await SendWithFluentEmailAsync(request, cancellationToken);

            EnsureEmailSendingResponse(response);
            RegisterSendTimeForRateLimit(request.RemoteIpAddress);
        }
        catch (RateLimitException)
        {
            // Ignored, no point to flood ourselves.
        }
        catch (Exception e)
        {
            var errorMessage = LogError(request, e);

            // This triggers a retry.
            throw new EmailSendException(errorMessage, e);
        }
    }

    private async Task<SendResponse> SendWithFluentEmailAsync(InquirySendEmailRequest request, CancellationToken cancellationToken)
    {
        var result = await _fluentEmail
            .To(_inquiriesSettings.DefaultEmail)
            .ReplyTo(request.Email)
            .SetFrom(_fluentEmailSettings.DefaultSender, request.Name)
            .Subject($"Smart Learning: {request.Name} a envoyé une question à partir du formulaire du site")
            .UsingTemplateFromEmbedded(Template, request, typeof(InquiryEmailService).Assembly)
            .SendAsync(cancellationToken);

        return result;
    }

    private static readonly object EnsureRateLimitLock = new();

    /// <summary>
    /// Ensures that <paramref name="remoteIpAddress" /> is not sending a mail within <see cref="InquiryOptions.RateLimitInSeconds" /> seconds.
    /// This method is thread safe.
    /// </summary>
    /// <param name="remoteIpAddress">The remote IP address of the sender.</param>
    /// <exception cref="RateLimitException">The IP address has already sent a inquiry within the allowed time frame.</exception>
    private void EnsureRateLimit(string remoteIpAddress)
    {
        lock (EnsureRateLimitLock)
        {
            var lastTimeSentEmail = _memoryCache.Get<DateTime?>(GetKey(remoteIpAddress));

            if (lastTimeSentEmail is not null)
            {
                throw new RateLimitException(remoteIpAddress);
            }
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
        lock (EnsureRateLimitLock)
        {
            _memoryCache.Set(GetKey(ipAddress), DateTime.UtcNow, TimeSpan.FromSeconds(_inquiriesSettings.RateLimitInSeconds));
        }
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
/// A service that sends in to the Smart Learning teams visitors' general inquiries.
/// </summary>
public interface IInquiryEmailService
{
    /// <summary>
    /// Sends in to Smart Learning team an email containing an inquiry of a visitor.
    /// The execution of the code retries on failures.
    /// This method is safe, i.e., it captures any encountered exceptions.
    /// </summary>
    /// <param name="request">Content of the inquiry.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns><see cref="InquirySendEmailResult"/> that represents the outcome of the operation. </returns>
    InquirySendEmailResult SendEmail(InquirySendEmailRequest request, CancellationToken cancellationToken = default);
}
