using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Smart.FA.Catalog.Web.Extensions;

/// <summary>
/// Serializer and Deserializer for the ModelState
/// </summary>
public static class ModelStateSerializer
{
    private class ModelStateTransferValue
    {
        public string Key { get; set; } = null!;

        public string AttemptedValue { get; set; } = null!;

        public object RawValue { get; set; } = null!;

        public ICollection<string> ErrorMessages { get; set; } = new List<string>();
    }

    public static string Serialize(ModelStateDictionary modelState)
    {
        var errorList = modelState
            .Select(kvp => new ModelStateTransferValue
            {
                Key = kvp.Key,
                AttemptedValue = kvp.Value?.AttemptedValue ?? string.Empty,
                RawValue = kvp.Value?.RawValue ?? string.Empty,
                ErrorMessages = kvp.Value?.Errors.Select(err => err.ErrorMessage).ToList() ?? new List<string>(),
            });

        return System.Text.Json.JsonSerializer.Serialize(errorList);
    }

    public static ModelStateDictionary Deserialize(string serializedErrorList)
    {
        var errorList = System.Text.Json.JsonSerializer.Deserialize<List<ModelStateTransferValue>>(serializedErrorList);
        var modelState = new ModelStateDictionary();

        foreach (var item in errorList ?? new List<ModelStateTransferValue>())
        {
            modelState.SetModelValue(item.Key, item.RawValue, item.AttemptedValue);
            foreach (var error in item.ErrorMessages)
            {
                modelState.AddModelError(item.Key, error);
            }
        }
        return modelState;
    }
}

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

        var modelState = ModelStateSerializer.Deserialize(serializedModelState);
        // Overwrite previous data stored with this Key in the ModelState with most recent one
        page.ModelState.Merge(modelState);
    }

    public void OnPageHandlerExecuting(PageHandlerExecutingContext context) { }

    public void OnPageHandlerExecuted(PageHandlerExecutedContext context) { }
}

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
        var modelState = ModelStateSerializer.Serialize(page.ModelState);
        // Use TempData to store the serialized errors across requests with the name of the page filter as key
        page.TempData[SerializeModelStateFilter.Key] = modelState;
        return actionResult;
    }
}
