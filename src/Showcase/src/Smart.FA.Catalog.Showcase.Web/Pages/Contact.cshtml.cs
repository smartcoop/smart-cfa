using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Smart.FA.Catalog.Showcase.Infrastructure.Mailing.Contact;

namespace Smart.FA.Catalog.Showcase.Web.Pages;

public class ContactModel : PageModel
{
    private readonly IInquiryEmailService _inquiryEmailService;

    [BindProperty]
    public InquirySendEmailRequest SendEmailRequest { get; set; } = null!;

    [TempData]
    public bool? SendingWasSuccessful { get; set; }

    public ContactModel(IInquiryEmailService inquiryEmailService)
    {
        _inquiryEmailService = inquiryEmailService;
    }

    public PageResult OnGet()
    {
        return Page();
    }

    public async Task<ActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        SendingWasSuccessful = await _inquiryEmailService.SendEmailAsync(SendEmailRequest);

        return SendingWasSuccessful is true ? RedirectToPage() : Page();
    }
}
