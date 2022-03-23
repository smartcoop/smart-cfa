using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Smart.FA.Catalog.Web.Extensions;

public static class ViewDataDictionaryExtensions
{
    public static void SetTitle(this ViewDataDictionary viewData, string? title)
    {
        if (!string.IsNullOrWhiteSpace(title))
        {
            viewData["Title"] = title;
        }
    }
}
