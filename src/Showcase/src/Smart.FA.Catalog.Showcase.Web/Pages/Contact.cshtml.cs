using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
        if (!ModelState.IsValid)
        {
            if (ModelState.ContainsKey(ModelStateErrorKeys.RateLimit))
            {
                ErrorMessage = string.Join(". ", ModelState[ModelStateErrorKeys.RateLimit]!.Errors.Select(e => e.ErrorMessage));
            }

            return Page();
        }

        try
        {
            _inquiryEmailService.SendEmail(SendEmailRequest);

            // There's no errors, we can therefore specify it.
            ErrorMessage = string.Empty;
        }
        catch (Exception)
        {
            // _inquiryEmailService.SendEmail logs any encountered exception.
            // No need to make a log here.
            ErrorMessage = ShowcaseResources.Contact_ErrorMessage;

            // This renders again the Page and therefore will fill in back the form.
            return Page();
        }

        return RedirectToPage();
    }
}
