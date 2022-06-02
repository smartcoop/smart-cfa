using FluentEmail.Core;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Smart.FA.Catalog.Showcase.Domain.Common.Options;

namespace Smart.FA.Catalog.Showcase.Infrastructure.Mailing.Inquiry.SmartLearningTeam;

/// <inheritdoc cref="ISmartLearningInquiryEmailService" />
public class SmartLearningTeamInquiryEmailService : InquiryEmailServiceBase<InquirySendEmailRequest, InquirySendEmailRequest>, ISmartLearningInquiryEmailService
{
    private const string TemplatePath = "Smart.FA.Catalog.Showcase.Infrastructure.Mailing.Inquiry.SmartLearningTeam.InquiryEmailTemplate.cshtml";

    private readonly InquiryOptions _inquirySettings;
    private readonly FluentEmailOptions _fluentEmailSettings;

    protected internal override string Template => TemplatePath;

    public SmartLearningTeamInquiryEmailService(ILogger<SmartLearningTeamInquiryEmailService> logger,
        IFluentEmail fluentEmail,
        IMemoryCache memoryCache,
        IOptions<FluentEmailOptions> fluentEmailOptions,
        IOptions<InquiryOptions> inquiryOptions) : base(logger, fluentEmail, memoryCache)
    {
        _inquirySettings = inquiryOptions.Value;
        _fluentEmailSettings = fluentEmailOptions.Value;
    }
    protected override string GetKey(string ipAddress) => "inquiry-rate-limit-" + ipAddress;

    protected override Task LoadDataAsync(InquirySendEmailRequest request, CancellationToken cancellationToken) => Task.CompletedTask;

    protected override Task PostEmailSendingAsync() => Task.CompletedTask;

    protected override string GetFromEmail() => _fluentEmailSettings.DefaultSender;

    protected override string GetFromName() => Request.Name;

    protected override int GetRateLimitInSeconds() => _inquirySettings.RateLimitInSeconds;

    protected override string GetReplyToRecipients() => Request.Email;

    protected override Task<InquirySendEmailRequest> GetTemplateModelAsync(InquirySendEmailRequest inquirySendEmailRequest) => Task.FromResult(inquirySendEmailRequest);

    protected override string GetToRecipients() => _inquirySettings.DefaultEmail;

    protected override string GetSubject() => $"Smart Learning: {Request.Name} a envoyé une question à partir du formulaire du site";

    private static readonly object RateLimitRock = new();

    protected override object GetRateLimitLock() => RateLimitRock;
}

/// <summary>
/// A service that sends in to the Smart Learning teams visitors' general inquiries.
/// </summary>
public interface ISmartLearningInquiryEmailService : IInquiryEmailService<InquirySendEmailRequest, InquirySendEmailRequest>
{
}
