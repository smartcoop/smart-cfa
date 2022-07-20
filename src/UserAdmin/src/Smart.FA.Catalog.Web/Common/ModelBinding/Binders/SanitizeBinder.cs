using Ganss.XSS;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Smart.FA.Catalog.Web.Common.ModelBinding.Binders;

public class SanitizeBinder : IModelBinder
{
    private readonly IHtmlSanitizer _htmlSanitizer;

    public SanitizeBinder(IHtmlSanitizer htmlSanitizer)
    {
        _htmlSanitizer = htmlSanitizer;
    }

    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        ArgumentNullException.ThrowIfNull(bindingContext);

        var modelName = bindingContext.ModelName;
        var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

        if (valueProviderResult == ValueProviderResult.None)
        {
            return Task.CompletedTask;
        }

        bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);
        var stringValue = valueProviderResult.FirstValue;

        try
        {
            SetBindingContextResult(bindingContext, stringValue, modelName, valueProviderResult);
        }
        catch (Exception exception)
        {
            // Conversion failed.
            bindingContext.ModelState.TryAddModelError(modelName, exception, bindingContext.ModelMetadata);
        }
        finally
        {
            bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);
        }


        return Task.CompletedTask;
    }

    private void SetBindingContextResult(ModelBindingContext bindingContext, string? stringValue, string modelName, ValueProviderResult valueProviderResult)
    {
        if (bindingContext.ModelMetadata.UnderlyingOrModelType == typeof(string))
        {
            stringValue = stringValue is not null ? _htmlSanitizer.Sanitize(stringValue) : null;
        }
        else
        {
            throw new NotSupportedException();
        }

        if (stringValue == null && !bindingContext.ModelMetadata.IsReferenceOrNullableType)
        {
            bindingContext.ModelState.TryAddModelError(
                modelName,
                bindingContext.ModelMetadata.ModelBindingMessageProvider.ValueMustNotBeNullAccessor(
                    valueProviderResult.ToString()));
        }
        else
        {
            bindingContext.Result = ModelBindingResult.Success(stringValue);
        }
    }
}
