namespace Smart.FA.Catalog.Showcase.Web.Extensions;

public static class StringHelper
{
    public static string ConserveLineBreaksInHtml(this string input) => input
        .Replace("\r\n", "<br />")
        .Replace("\n", "<br />");
}
