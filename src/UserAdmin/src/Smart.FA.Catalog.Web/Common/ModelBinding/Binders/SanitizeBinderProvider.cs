using Ganss.XSS;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Smart.FA.Catalog.Application;

namespace Smart.FA.Catalog.Web.Common.ModelBinding.Binders;

public class SanitizeBinderProvider : IModelBinderProvider
{
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        if (context.Metadata.UnderlyingOrModelType == typeof(string) &&
            context.Metadata is DefaultModelMetadata defaultModel &&
            defaultModel.Attributes.PropertyAttributes?.Any(attribute => attribute.GetType() == typeof(SanitizedAttribute)) == true)
        {
            return new SanitizeBinder(context.Services.GetRequiredService<IHtmlSanitizer>());
        }

        return null;
    }
}
