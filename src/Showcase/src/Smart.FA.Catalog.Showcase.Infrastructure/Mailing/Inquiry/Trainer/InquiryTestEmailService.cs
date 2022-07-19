using FluentEmail.Core;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Smart.FA.Catalog.Showcase.Domain.Common.Options;

namespace Smart.FA.Catalog.Showcase.Infrastructure.Mailing.Inquiry.Trainer;

/// <summary>
/// Test Trainer Inquiry mail service with sending of the mail to a specific address used for debugging and general testing.
/// </summary>
public class InquiryTestEmailService : TrainerInquirySendEmailService
{
    private readonly TestInquiryOptions _testInquirySettings;

    public InquiryTestEmailService(
        ILogger<InquiryTestEmailService> logger,
        IServiceProvider serviceProvider,
        IFluentEmail fluentEmail,
        IMemoryCache memoryCache,
        IOptions<FluentEmailOptions> fluentEmailOptions,
        IOptions<InquiryOptions> inquiryOptions,
        IOptions<TestInquiryOptions> testInquiryOptions) : base(logger, serviceProvider, fluentEmail, memoryCache, fluentEmailOptions, inquiryOptions)
    {
        _testInquirySettings = testInquiryOptions.Value;
    }

    protected override string GetToRecipients() => _testInquirySettings.DefaultEmail;
}
