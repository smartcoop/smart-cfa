using FluentEmail.Core;
using FluentEmail.Core.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Smart.FA.Catalog.Showcase.Domain.Common.Enums;
using Smart.FA.Catalog.Showcase.Domain.Common.Exceptions;
using Smart.FA.Catalog.Showcase.Domain.Common.Options;

namespace Smart.FA.Catalog.Showcase.Infrastructure.Mailing.Inquiry;

public abstract class InquiryEmailServiceBase
{
    protected static object RateLimitLock = new();
}

public abstract class InquiryEmailServiceBase<TInquiryRequest, TTemplateModel> : IInquiryEmailService<TInquiryRequest, TTemplateModel>
    where TInquiryRequest : InquirySendEmailRequest
{
    /// <summary>
    /// Path to the email template.
    /// </summary>
    protected abstract string Template { get; }

    protected abstract string GetKey(string ipAddress);
    protected abstract string GetFromEmail();
    protected abstract string GetFromName();
    protected abstract int GetRateLimitInSeconds();
    protected abstract string GetReplyToRecipients();
    protected abstract string GetToRecipients();
    protected abstract Task LoadDataAsync(TInquiryRequest request, CancellationToken cancellationToken);
    protected abstract Task PostEmailSendingAsync();

    // This ensures classes that inherits from this class have their own lock object.
    protected abstract object GetRateLimitLock();

    protected abstract Task<TTemplateModel> GetTemplateModelAsync(TInquiryRequest inquirySendEmailRequest);

    private readonly ILogger<InquiryEmailServiceBase<TInquiryRequest, TTemplateModel>> _logger;
    private readonly IFluentEmail _fluentEmail;

    protected IMemoryCache MemoryCache;

    protected InquiryEmailServiceBase(
        ILogger<InquiryEmailServiceBase<TInquiryRequest, TTemplateModel>> logger,
        IFluentEmail fluentEmail,
        IMemoryCache memoryCache)
    {
        _logger = logger;
        _fluentEmail = fluentEmail;
        MemoryCache = memoryCache;
    }

    /// <inheritdoc />
    public Task<InquirySendEmailResult> SendEmailAsync(TInquiryRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            // Will throw an exception if an email was already sent in the allowed time frame.
            EnsureRateLimit(request.RemoteIpAddress);
            RegisterSendTimeForRateLimit(request.RemoteIpAddress);
            Task.Run(async () => await SendWithRetriesAsync(request, cancellationToken), cancellationToken);
            return Task.FromResult(InquirySendEmailResult.Success);
        }
        catch (RateLimitException)
        {
            return Task.FromResult(InquirySendEmailResult.TooManyRequests);
        }
    }

    private async Task SendWithRetriesAsync(TInquiryRequest request, CancellationToken cancellationToken = default)
    {
        // 11, 12, 17, 29, 56, 117, 252, 550 seconds between retries.
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
    /// Computes the delay in milliseconds to wait until the next retry.
    /// This methods implements exponential backoff.
    /// </summary>
    /// <param name="retryAttempt">Current retry attempt.</param>
    /// <returns>The delay to wait until the next retry attempt in milliseconds.</returns>
    protected virtual int DelayToWaitBetweenRetriesInMilliseconds(int retryAttempt) =>
        (int) (Math.Max(10 - retryAttempt, 0) + Math.Pow(2.2, Math.Min(retryAttempt, 40))) * 1_000;

    /// <summary>
    /// Sends an email inquiry to a given email address.
    /// </summary>
    /// <param name="request">Content of the inquiry.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <return>A <see cref="Task" /> that represents the asynchronous operation.</return>
    /// <exception cref="EmailSendException">If any failure happens.</exception>
    public async Task SendEmailInternalAsync(TInquiryRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            await LoadDataAsync(request, cancellationToken);
            SetRequest(request);
            var response = await SendWithFluentEmailAsync(request, cancellationToken);
            EnsureEmailSendingResponse(response);
            await PostEmailSendingAsync();
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

    private void SetRequest(TInquiryRequest request)
    {
        Request = request;
    }

    private void SetTo()
    {
        _fluentEmail.To(GetToRecipients());
    }

    private void SetReplyTo()
    {
        _fluentEmail.ReplyTo(GetReplyToRecipients());
    }

    private void SetSubject()
    {
        _fluentEmail.Subject(GetSubject());
    }

    protected abstract string GetSubject();

    private void SetFrom()
    {
        _fluentEmail.SetFrom(GetFromEmail(), GetFromName());
    }

    private async Task<SendResponse> SendWithFluentEmailAsync(TInquiryRequest request, CancellationToken cancellationToken)
    {
        SetFrom();
        SetTo();
        SetReplyTo();
        SetSubject();
        await SetTemplateAsync(request);

        return await _fluentEmail.SendAsync(cancellationToken);
    }

    protected TInquiryRequest Request { get; set; }

    private async Task SetTemplateAsync(TInquiryRequest request)
    {
        var model = await GetTemplateModelAsync(request);

        _fluentEmail.UsingTemplateFromEmbedded(Template, model, typeof(InquiryEmailServiceBase<TInquiryRequest, TTemplateModel>).Assembly);
    }

    /// <summary>
    /// Ensures that <paramref name="remoteIpAddress" /> is not sending a mail within <see cref="InquiryOptions.RateLimitInSeconds" /> seconds.
    /// This method is thread safe.
    /// </summary>
    /// <param name="remoteIpAddress">The remote IP address of the sender.</param>
    /// <exception cref="RateLimitException">The IP address has already sent a inquiry within the allowed time frame.</exception>
    protected void EnsureRateLimit(string remoteIpAddress)
    {
        lock (GetRateLimitLock())
        {
            var lastTimeSentEmail = MemoryCache.Get<DateTime?>(GetKey(remoteIpAddress));

            if (lastTimeSentEmail is not null)
            {
                throw new RateLimitException(remoteIpAddress);
            }
        }
    }

    private void RegisterSendTimeForRateLimit(string ipAddress)
    {
        lock (GetRateLimitLock())
        {
            MemoryCache.Set(GetKey(ipAddress), DateTime.UtcNow, TimeSpan.FromSeconds(GetRateLimitInSeconds()));
        }
    }

    private void EnsureEmailSendingResponse(SendResponse result)
    {
        if (!result.Successful)
        {
            throw new Exception(string.Join(", ", result.ErrorMessages));
        }
    }

    private string LogError(TInquiryRequest request, Exception e)
    {
        var errorMessage = $"An error occurred while sending a mail from {request.Email} with message {request.Message}";
        _logger.LogError(e, errorMessage);
        return errorMessage;
    }
}
