using FluentEmail.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Smart.FA.Catalog.Showcase.Domain.Common.Options;
using Smart.FA.Catalog.Showcase.Domain.Models;
using Smart.FA.Catalog.Showcase.Infrastructure.Data;

namespace Smart.FA.Catalog.Showcase.Infrastructure.Mailing.Inquiry.Trainer;

public class TrainerInquirySendEmailService : InquiryEmailServiceBase<TrainerInquirySendEmailRequest, TrainerInquirySendEmailTemplateModel>, ITrainerInquirySendEmailService
{
    private const string TemplatePath = "Smart.FA.Catalog.Showcase.Infrastructure.Mailing.Inquiry.Trainer.TrainerInquiryTemplate.cshtml";

    private readonly InquiryOptions _inquirySettings;
    private readonly FluentEmailOptions _fluentEmailSettings;
    private readonly IServiceProvider _serviceProvider;

    private TrainerDetails _trainer = null!;

    public TrainerInquirySendEmailService(
        ILogger<InquiryEmailServiceBase<TrainerInquirySendEmailRequest, TrainerInquirySendEmailTemplateModel>> logger,
        IServiceProvider serviceProvider,
        IFluentEmail fluentEmail,
        IMemoryCache memoryCache,
        IOptions<FluentEmailOptions> fluentEmailOptions,
        IOptions<InquiryOptions> inquiryOptions) : base(logger, fluentEmail, memoryCache)
    {
        _serviceProvider = serviceProvider;
        _fluentEmailSettings = fluentEmailOptions.Value;
        _inquirySettings = inquiryOptions.Value;
    }

    protected internal override string Template => TemplatePath;

    protected override async Task LoadDataAsync(TrainerInquirySendEmailRequest request, CancellationToken cancellationToken)
    {
        // We need the email of the trainer.
        EnsureTrainingIdIsDefined(request);

        // If we were to inject the DbContext in the controller we would get an exception as the DI would dispose it when the HTTP request is done.
        await RetrieveTrainerAsync(request, cancellationToken);

        // Someone could try sending random id.
        // If the id exists but is a trainer not having one more published training this prevent sending a mail to that person.
        EnsureTrainerExists();
    }

    private static void EnsureTrainingIdIsDefined(TrainerInquirySendEmailRequest request)
    {
        if (!request.TrainerId.HasValue)
        {
            throw new InvalidOperationException("Id of the Trainer is not defined");
        }
    }

    private async Task RetrieveTrainerAsync(TrainerInquirySendEmailRequest request, CancellationToken cancellationToken)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        await using var dbContext = scope.ServiceProvider.GetRequiredService<CatalogShowcaseContext>();
        _trainer = await dbContext.TrainerDetails.FirstAsync(trainer => trainer.Id == request.TrainerId, cancellationToken);
    }

    private void EnsureTrainerExists()
    {
        if (_trainer is null)
        {
            throw new InvalidOperationException("Attempted to send a mail to a trainer that either doesn't exist or hasn't at least one published training");
        }
    }

    protected override Task PostEmailSendingAsync() => Task.CompletedTask;

    protected override string GetKey(string ipAddress) => $"trainer-inquiry-{ipAddress}";

    protected override string GetFromEmail() => _fluentEmailSettings.DefaultSender;

    protected override string GetFromName() => Request.Name;

    protected override int GetRateLimitInSeconds() => _inquirySettings.RateLimitInSeconds;

    protected override string GetReplyToRecipients() => Request.Email;

    protected override string GetToRecipients() => _trainer.Email!;

    protected override Task<TrainerInquirySendEmailTemplateModel> GetTemplateModelAsync(TrainerInquirySendEmailRequest inquirySendEmailRequest) =>
        Task.FromResult(new TrainerInquirySendEmailTemplateModel()
        {
            Email = inquirySendEmailRequest.Email,
            Name = inquirySendEmailRequest.Name,
            Message = inquirySendEmailRequest.Message,
            TrainerName = $"{_trainer.FirstName} {_trainer.LastName}"
        });

    protected override string GetSubject() => "Vous avez recu une question sur Smart Learning";

    private static readonly object RateLimitLock = new();

    protected override object GetRateLimitLock() => RateLimitLock;
}

public interface ITrainerInquirySendEmailService : IInquiryEmailService<TrainerInquirySendEmailRequest, TrainerInquirySendEmailTemplateModel>
{
}
