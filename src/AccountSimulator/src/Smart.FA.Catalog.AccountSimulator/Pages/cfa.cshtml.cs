using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Smart.FA.Catalog.AccountSimulator.Pages;

public class cfa : PageModel
{
    public void OnGet(string? id)
    {
        Response.Cookies.Append("userid", id);
        HttpContext.ProxyRedirect("/", id, AccountataFactory.GetByUserId(id).Serialize());
    }
}
