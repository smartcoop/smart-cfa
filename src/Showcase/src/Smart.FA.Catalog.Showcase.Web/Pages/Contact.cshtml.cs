using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Smart.FA.Catalog.Showcase.Domain.Common.Enums;
using Smart.FA.Catalog.Showcase.Infrastructure.Mailing.Inquiry;
using Smart.FA.Catalog.Showcase.Infrastructure.Mailing.Inquiry.SmartLearningTeam;

namespace Smart.FA.Catalog.Showcase.Web.Pages;

public class ContactModel : PageModel
{
    private readonly ISmartLearningInquiryEmailService _inquiryEmailService;

    [BindProperty]
    public InquirySendEmailRequest SendEmailRequest { get; set; } = null!;

    public InquirySendEmailResult? SendEmailResult { get; set; }

    public ContactModel(ISmartLearningInquiryEmailService inquiryEmailService)
    {
        _inquiryEmailService = inquiryEmailService;
    }

    public PageResult OnGet()
    {
        return Page();
    }

    public async Task<ActionResult> OnPostAsync()
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            AddToRequestSenderRemoteIpAddress();
            SendEmailResult = await _inquiryEmailService.SendEmailAsync(SendEmailRequest);
        }
        catch (Exception)
        {
            // No need to perform any logging here as _inquiryEmailService.SendEmail logs any encountered exception.
            // The method is supposed to catch any exceptions but better be safe than sorry.
            SendEmailResult = InquirySendEmailResult.Failure;
        }

        // If ErrorMessage has a value this means we are in an error/invalid state.
        // If it is the case, render the Page again with Page() method.
        return Page();
    }

    private void AddToRequestSenderRemoteIpAddress()
    {
        SendEmailRequest.RemoteIpAddress = HttpContext.Connection.RemoteIpAddress!.ToString();
    }
}
