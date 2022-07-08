using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Smart.FA.Catalog.Web.Filters;

namespace Smart.FA.Catalog.Web.Extensions;

/// <summary>
/// Extension method for ASP.NET redirect results to store the ModelState across requests in TempData
/// (edited source: https://speednet.pl/blog/post-redirect-get-in-asp-net-razor-pages)
/// </summary>
public static class KeepTempDataResultExtensions
{
    /// <summary>
    /// Stores the ModelState of the page in TempData after a redirect call
    /// </summary>
    /// <param name="actionResult">IKeepTempDataResult implemented on all standard ASP.NET redirect call</param>
    /// <param name="page">The page with the ModelState that need to be preserved</param>
    /// <returns></returns>
    public static IKeepTempDataResult WithModelStateOf(this IKeepTempDataResult actionResult, PageModel page)
    {
        // If the ModelState is valid, there is no point to serialize anything
        if (page.ModelState.IsValid)
        {
            return actionResult;
        }

        // Serialize all ModelState errors
        var modelState = page.ModelState.Serialize();
        // Use TempData to store the serialized errors across requests with the name of the page filter as key
        page.TempData[SerializeModelStateFilter.Key] = modelState;
        return actionResult;
    }
}
