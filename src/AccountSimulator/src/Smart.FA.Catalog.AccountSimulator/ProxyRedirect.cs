namespace Smart.FA.Catalog.AccountSimulator;

public static class ProxyExtensions
{
    public static void ProxyRedirect(this HttpContext context, string page, string userId, string customData)
    {
        context.Response.Headers.Add("userId", userId);
        context.Response.Headers.Add("smartApplication", "Account");
        context.Response.Headers.Add("customData", customData);
        context.Response.Headers.Add("X-Accel-Redirect", "@myinternalapplication");
        context.Response.Headers.Add("X-Real-Location", page);
    }
}
