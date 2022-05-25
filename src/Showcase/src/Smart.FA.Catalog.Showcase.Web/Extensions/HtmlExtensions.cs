using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Smart.FA.Catalog.Showcase.Web.Extensions;

public static class StringHelper
{
    public static IHtmlContent PreserveLineBreaks(this IHtmlHelper input, string inputText)
    {
        inputText = inputText
            .Replace("\r\n", "<br />")
            .Replace("\n", "<br />");
        return new HtmlString(inputText);
    }
}
