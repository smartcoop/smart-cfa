namespace Smart.FA.Catalog.AccountSimulator;

public static class ProxyExtensions
{
    public static void ProxyRedirect(this HttpContext context, string page, string userId)
    {
        context.Response.Headers.Add("user_id", userId);
        context.Response.Headers.Add("app_name", "Account");
        context.Response.Headers.Add("X-Accel-Redirect", "@myinternalapplication");
        context.Response.Headers.Add("X-Real-Location", page);
    }
}
