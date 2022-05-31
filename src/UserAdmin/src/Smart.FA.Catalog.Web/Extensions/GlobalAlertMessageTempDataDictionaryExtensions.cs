using Smart.Design.Razor.TagHelpers.Alert;
using Smart.FA.Catalog.Core.Extensions;

namespace Microsoft.AspNetCore.Mvc.ViewFeatures;

/// <summary>
/// Provides extension methods on <see cref="ITempDataDictionary" /> to transit messages between HTTP requests that should be rendered in a <see cref="AlertTagHelper" />.
/// </summary>
public static class GlobalAlertMessageTempDataDictionaryExtensions
{
    private const string GlobalMessageKey = nameof(GlobalAlertMessage);

    public static void AddGlobalAlertMessage(this ITempDataDictionary tempData, string? message, AlertStyle style)
    {
        if (!string.IsNullOrEmpty(message))
        {
            AddGlobalAlertMessage(tempData, new GlobalAlertMessage(style, message));
        }
    }

    public static void AddGlobalAlertMessage(this ITempDataDictionary tempData, GlobalAlertMessage globalAlertMessage)
    {
        if (!string.IsNullOrEmpty(globalAlertMessage?.Message))
        {
            tempData[GlobalMessageKey] = globalAlertMessage.ToJson();
        }
    }

    public static bool HasGlobalAlertMessage(this ITempDataDictionary tempData)
    {
        return tempData.ContainsKey(GlobalMessageKey);
    }

    public static GlobalAlertMessage GetGlobalAlertMessage(this ITempDataDictionary tempData)
    {
        if (tempData.TryGetValue(GlobalMessageKey, out var obj) &&
            obj is string serializedGlobalAlertMessage)
        {
            return serializedGlobalAlertMessage.FromJson<GlobalAlertMessage>()!;
        }

        return new GlobalAlertMessage(AlertStyle.Default, string.Empty);
    }
}

public class GlobalAlertMessage
{
    public AlertStyle Style { get; set; }

    public string? Message { get; set; }

    public GlobalAlertMessage(AlertStyle style, string? message)
    {
        Style = style;
        Message = message;
    }

    public override string? ToString()
    {
        return Message;
    }
}
