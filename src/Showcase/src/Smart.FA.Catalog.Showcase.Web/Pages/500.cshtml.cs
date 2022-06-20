using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Smart.FA.Catalog.Showcase.Web.Pages;

public class ServerErrorPage : PageModel
{
    public const string ErrorTitleKey = nameof(ErrorTitle);

    public const string ErrorMessageKey = nameof(ErrorMessage);

    public string ErrorTitle { get; set; } = null!;

    public string ErrorMessage { get; set; } = null!;

    public PageResult OnGet()
    {
        SetTitle();
        SetMessage();

        return Page();
    }

    private void SetTitle()
    {
        if (TempData.ContainsKey(ErrorTitleKey))
        {
            ErrorTitle = TempData[ErrorTitle]!.ToString()!;
        }
        else if (HttpContext.Items.ContainsKey(ErrorTitleKey))
        {
            ErrorTitle = HttpContext.Items[ErrorTitleKey]!.ToString()!;
        }
        else
        {
            ErrorTitle = ShowcaseResources.GenericNotFoundPageTitle;
        }
    }

    private void SetMessage()
    {
        if (string.IsNullOrEmpty(TempData[ErrorMessageKey]?.ToString()))
        {
            return;
        }

        if (TempData.ContainsKey(ErrorMessageKey))
        {
            ErrorMessage = TempData[ErrorMessageKey]!.ToString()!;
        }
        else if (HttpContext.Items.ContainsKey(ErrorMessageKey))
        {
            ErrorMessage = HttpContext.Items[ErrorMessageKey]!.ToString()!;
        }
        else
        {
            ErrorMessage = ShowcaseResources.UnexpectedError;
        }
    }
}
