namespace Microsoft.AspNetCore.Mvc.ViewFeatures;

public static class TempDataExtensions
{
    /// <summary>
    /// Gets the value of the <paramref name="key"/> and attempt to convert it to <typeparamref name="T" />.
    /// If <paramref name="value" /> cannot be converted to type <typeparamref name="T" /> then <see langword="default" /> is returned.
    /// </summary>
    /// <returns></returns>
    public static bool TryGetConvertedValue<T>(this ITempDataDictionary tempData, string key, out T? value)
    {
        var keyExists = tempData.TryGetValue(key, out var tempDataValue);
        value = keyExists && tempDataValue is T typedValue ? typedValue : default;
        return keyExists;
    }
}
