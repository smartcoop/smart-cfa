using Core.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.Pages.Admin;

namespace Web.Pages;

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
[IgnoreAntiforgeryToken]
public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    public CustomIdentity? Identity { get; set; }
    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public IActionResult OnGet()
    {
        ViewData[nameof(SideMenuItem)] = SideMenuItem.MyTrainings;
        Identity = HttpContext.User.Identity as CustomIdentity;
        // if (!User.Identity!.IsAuthenticated)
        //     return RedirectToPage("Admin/Account/Index");
        return Page();
    }
}
