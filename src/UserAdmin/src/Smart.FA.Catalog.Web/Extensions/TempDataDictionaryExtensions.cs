using Smart.Design.Razor.TagHelpers.Alert;
using Smart.FA.Catalog.Core.Extensions;

namespace Microsoft.AspNetCore.Mvc.ViewFeatures;

public static class TempDataDictionaryExtensions
{
    private const string GlobalMessageKey = nameof(GlobalBannerMessage);

    public static void AddGlobalBannerMessage(this ITempDataDictionary tempData, string? message, AlertStyle style)
    {
        if (!string.IsNullOrEmpty(message))
        {
            AddGlobalBannerMessage(tempData, new GlobalBannerMessage(style, message));
        }
    }

    public static void AddGlobalBannerMessage(this ITempDataDictionary tempData, GlobalBannerMessage globalBannerMessage)
    {
        if (!string.IsNullOrEmpty(globalBannerMessage?.Message))
        {
            tempData[GlobalMessageKey] = globalBannerMessage.ToJson();
        }
    }

    public static bool HasGlobalBannerMessage(this ITempDataDictionary tempData)
    {
        return tempData.ContainsKey(GlobalMessageKey);
    }

    public static GlobalBannerMessage GetGlobalBannerMessage(this ITempDataDictionary tempData)
    {
        if (tempData.TryGetValue(GlobalMessageKey, out var obj) &&
            obj is string serializedGlobalBannerMessage)
        {
            return serializedGlobalBannerMessage.FromJson<GlobalBannerMessage>()!;
        }

        return new GlobalBannerMessage(AlertStyle.Default, string.Empty);
    }
}

public class GlobalBannerMessage
{
    public AlertStyle Style { get; set; }

    public string? Message { get; set; }

    public GlobalBannerMessage(AlertStyle style, string? message)
    {
        Style = style;
        Message = message;
    }

    public override string? ToString()
    {
        return Message;
    }
}
