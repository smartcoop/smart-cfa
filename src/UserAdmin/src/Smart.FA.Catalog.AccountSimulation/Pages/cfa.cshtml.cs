using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Smart.FA.Catalog.AccountSimulation.Options;

namespace Smart.FA.Catalog.AccountSimulation.Pages;

public class cfa : PageModel
{
    public void OnGet(int? id)
    {
        Response.Cookies.Append("user-id", id.ToString());
        HttpContext.ProxyRedirect("/", id.ToString());
    }

    // public void OnGet(int? id)
    // {
    //     var url =HttpContext.Request.GetEncodedUrl().ToLower().Replace("/cfa", "");
    //
    //     Response.Cookies.Append("user-id", id.ToString());
    //     HttpContext.ProxyRedirect("admin/myprofile", id.ToString());
    // }

}
