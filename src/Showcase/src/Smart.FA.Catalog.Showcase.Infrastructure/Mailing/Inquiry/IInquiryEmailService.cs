using Smart.FA.Catalog.Showcase.Domain.Common.Enums;

namespace Smart.FA.Catalog.Showcase.Infrastructure.Mailing.Inquiry;

public interface IInquiryEmailService<in TInquiryRequest, TTemplateModel> where TInquiryRequest : InquirySendEmailRequest
{
    /// <summary>
    /// Sends in to Smart Learning team an email containing an inquiry of a visitor.
    /// The execution of the code retries on failures.
    /// This method is safe, i.e., it captures any encountered exceptions.
    /// </summary>
    /// <param name="request">Content of the inquiry.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns><see cref="InquirySendEmailResult"/> that represents the outcome of the operation. </returns>
    Task<InquirySendEmailResult> SendEmailAsync(TInquiryRequest request, CancellationToken cancellationToken = default);
}
