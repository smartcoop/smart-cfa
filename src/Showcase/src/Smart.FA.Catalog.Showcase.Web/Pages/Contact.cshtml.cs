using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Smart.FA.Catalog.Showcase.Domain.Common.Enums;
using Smart.FA.Catalog.Showcase.Infrastructure.Mailing.Contact;
using Smart.FA.Catalog.Showcase.Web.PageFilters;

namespace Smart.FA.Catalog.Showcase.Web.Pages;

[RateLimit(1, 60)]
public class ContactModel : PageModel
{
    private readonly IInquiryEmailService _inquiryEmailService;

    [BindProperty]
    public InquirySendEmailRequest SendEmailRequest { get; set; } = null!;

    [TempData]
    public string? ErrorMessage { get; set; }

    public ContactModel(IInquiryEmailService inquiryEmailService)
    {
        _inquiryEmailService = inquiryEmailService;
    }

    public PageResult OnGet()
    {
        return Page();
    }

    public ActionResult OnPost()
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            AddToRequestSenderRemoteIpAddress();
            var result = _inquiryEmailService.SendEmail(SendEmailRequest);
            SetErrorMessageBySendEmailResult(result);
        }
        catch (Exception)
        {
            // No need to perform any logging here as _inquiryEmailService.SendEmail logs any encountered exception.
            // The method is supposed to catch any exceptions but better be safe than sorry.
            ErrorMessage = ShowcaseResources.Contact_ErrorMessage;
        }

        // If ErrorMessage has a value this means we are in an error/invalid state.
        // If it is the case, render the Page again with Page() method.
        return string.IsNullOrEmpty(ErrorMessage) ? RedirectToPage() : Page();
    }

    private void AddToRequestSenderRemoteIpAddress()
    {
        SendEmailRequest.RemoteIpAddress = HttpContext.Connection.RemoteIpAddress!.ToString();
    }

    private void SetErrorMessageBySendEmailResult(InquirySendEmailResult result)
    {
        ErrorMessage = result switch
        {
            InquirySendEmailResult.TooManyRequest => ShowcaseResources.YouReachedRateLimit,
            InquirySendEmailResult.Failure => ShowcaseResources.Contact_ErrorMessage,
            InquirySendEmailResult.Ok => string.Empty,
            _ => ShowcaseResources.Contact_ErrorMessage
        };
    }
}
