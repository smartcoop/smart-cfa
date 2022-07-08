using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Smart.FA.Catalog.Web.Extensions;

/// <summary>
/// Serializer and Deserializer for the ModelState
/// </summary>
public static class ModelStateExtensions
{
    private class ModelStateTransferValue
    {
        public string Key { get; set; } = null!;

        public string? AttemptedValue { get; set; }

        public object? RawValue { get; set; }

        public ICollection<string> ErrorMessages { get; set; } = new List<string>();
    }

    public static string Serialize(this ModelStateDictionary modelState)
    {
        var errorList = modelState
            .Select(kvp => new ModelStateTransferValue
            {
                Key = kvp.Key,
                AttemptedValue = kvp.Value?.AttemptedValue,
                RawValue = kvp.Value?.RawValue,
                ErrorMessages = kvp.Value?.Errors.Select(err => err.ErrorMessage).ToList() ?? new List<string>(),
            });

        return System.Text.Json.JsonSerializer.Serialize(errorList);
    }

    public static ModelStateDictionary Deserialize(this string serializedErrorList)
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
