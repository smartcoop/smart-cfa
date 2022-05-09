using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Smart.FA.Catalog.AccountSimulator.Pages;

public class IndexModel : PageModel
{
    [BindProperty] public string SelectedUser { get; set; }

    public IndexModel(ILoggerFactory loggerFactory)
    {
    }

    public ActionResult OnPostRedirect(string url)
    {
        return RedirectToPage("/cfa", new { id = SelectedUser });
    }
}
