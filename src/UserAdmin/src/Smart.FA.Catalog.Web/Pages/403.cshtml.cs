using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Smart.FA.Catalog.Web.Options;

namespace Smart.FA.Catalog.Web.Pages;

public class ForbiddenPageModel : PageModel
{
    public UrlOptions UrlOptions { get; }

    public const string ErrorTitleKey = nameof(ErrorTitle);

    public const string ErrorMessageKey = nameof(ErrorMessage);

    public string ErrorTitle { get; set; } = null!;

    public string ErrorMessage { get; set; } = null!;

    public ForbiddenPageModel(IOptions<UrlOptions> urlOptions)
    {
        UrlOptions = urlOptions.Value;
    }

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
            return;
        }

        if (HttpContext.Items.ContainsKey(ErrorTitleKey))
        {
            ErrorTitle = HttpContext.Items[ErrorTitleKey]!.ToString()!;
            return;
        }

        ErrorTitle = CatalogResources.Sorry;
    }

    private void SetMessage()
    {
        if (!string.IsNullOrEmpty(TempData[ErrorMessageKey]?.ToString()))
        {
            return;
        }

        if (TempData.ContainsKey(ErrorMessageKey))
        {
            ErrorMessage = TempData[ErrorMessageKey]!.ToString()!;
            return;
        }

        if (HttpContext.Items.ContainsKey(ErrorMessageKey))
        {
            ErrorMessage = HttpContext.Items[ErrorMessageKey]!.ToString()!;
            return;
        }

        ErrorMessage = CatalogResources.PageForbidden;
    }
}
