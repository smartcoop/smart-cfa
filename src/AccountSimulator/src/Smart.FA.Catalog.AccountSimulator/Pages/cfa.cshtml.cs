using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Smart.FA.Catalog.AccountSimulator.Pages;

public class cfa : PageModel
{
    private readonly IAccountDataHeaderSerializer _accountDataHeaderSerializer;

    public cfa(IAccountDataHeaderSerializer accountDataHeaderSerializer)
    {
        _accountDataHeaderSerializer = accountDataHeaderSerializer;
    }

    public void OnGet(string? id)
    {
        Response.Cookies.Append("userid", id);
        HttpContext.ProxyRedirect("/", id, _accountDataHeaderSerializer.Serialize(_accountDataHeaderSerializer.GetByUserId(id)));
    }
}
