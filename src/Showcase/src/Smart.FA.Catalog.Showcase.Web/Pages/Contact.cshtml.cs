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

    public ActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            _inquiryEmailService.SendEmail(SendEmailRequest);
            SendingWasSuccessful = true;
        }
        catch (Exception)
        {
            // _inquiryEmailService.SendEmail logs any encountered exception.
            SendingWasSuccessful = false;
            return Page();
        }

        return RedirectToPage();
    }
}
