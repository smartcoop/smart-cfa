using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Smart.FA.Catalog.Web.Extensions;

namespace Smart.FA.Catalog.Web.Filters;

/// <summary>
/// Filter applied on a request on a PageModel to deserialize the ModelState stored in TempData
/// </summary>
public class SerializeModelStateFilter : IPageFilter
{
    public const string Key = $"{nameof(SerializeModelStateFilter)}Data";

    public void OnPageHandlerSelected(PageHandlerSelectedContext context)
    {
        // If the call is not made on a PageModel, there is no point to go further since there are no ModelState
        if (!(context.HandlerInstance is PageModel page))
        {
            return;
        }

        var serializedModelState = page.TempData[Key] as string;
        if (string.IsNullOrEmpty(serializedModelState))
        {
            return;
        }

        var modelState = serializedModelState.Deserialize();
        // Overwrite previous data stored with this Key in the ModelState with most recent one
        page.ModelState.Merge(modelState);
    }

    public void OnPageHandlerExecuting(PageHandlerExecutingContext context) { }

    public void OnPageHandlerExecuted(PageHandlerExecutedContext context) { }
}
