using System.Threading;
using System.Threading.Tasks;
using FluentEmail.Core;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Smart.FA.Catalog.Showcase.Infrastructure.Mailing.Inquiry;

namespace Smart.FA.Catalog.Showcase.Infrastructure.Tests.Mailing.Common;

public class TestInquiryEmailService : InquiryEmailServiceBase<TestEmailRequest, object>
{
    private readonly int _rateLimitInSeconds;

    public TestInquiryEmailService(
        ILogger<InquiryEmailServiceBase<TestEmailRequest, object>> logger,
        IFluentEmail fluentEmail,
        IMemoryCache memoryCache,
        int rateLimitInSeconds) : base(logger, fluentEmail, memoryCache)
    {
        _rateLimitInSeconds = rateLimitInSeconds;
    }

    protected internal override string Template { get; }

    protected override string GetKey(string ipAddress)
    {
        return string.Empty;
    }

    protected override string GetFromEmail()
    {
        return string.Empty;
    }

    protected override string GetFromName()
    {
        return string.Empty;
    }

    protected override int GetRateLimitInSeconds()
    {
        return _rateLimitInSeconds;
    }

    protected override string GetReplyToRecipients()
    {
        return string.Empty;
    }

    protected override string GetToRecipients()
    {
        return string.Empty;
    }

    protected override Task LoadDataAsync(TestEmailRequest request, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    protected override Task PostEmailSendingAsync()
    {
        return Task.CompletedTask;
    }

    private static readonly object Lock = new();

    protected override object GetRateLimitLock()
    {
        return Lock;
    }

    protected override Task<object> GetTemplateModelAsync(TestEmailRequest inquirySendTestEmailRequest)
    {
        return Task.FromResult(new object());
    }

    protected override string GetSubject()
    {
        return string.Empty;
    }
}
