using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Smart.FA.Catalog.Web.Extensions;

namespace Smart.FA.Catalog.Web.Pages;

public abstract class PageModelBase : PageModel
{
    /// <summary>
    /// Redirects to the 404 page by producing a not found response result.
    /// Also set customized strings for the title and message that will be handled by the 404 page.
    /// </summary>
    /// <param name="title">Optional title that will be displayed on the 404 page.</param>
    /// <param name="message">Optional content message that will be displayed on the 404 page.</param>
    /// <returns>The created <see cref="NotFoundResult"/> for the response.</returns>
    public NotFoundResult RedirectToNotFound(string? title = null, string? message = null)
    {
        TempData["ErrorTitle"] = title;
        TempData["ErrorMessage"] = message;

        return NotFound();
    }

    /// <summary>
    /// Redirects to page (http code 302) while also preserving the ModelState using TempData
    /// </summary>
    public IActionResult RedirectAndPreserveModelState()
    {
        return RedirectToPage().WithModelStateOf(this);
    }
}
