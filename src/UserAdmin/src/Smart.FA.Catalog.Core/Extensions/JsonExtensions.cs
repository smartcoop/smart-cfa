using System.Text.Json;

namespace Smart.FA.Catalog.Core.Extensions;

public static class JsonExtensions
{
    /// <inheritdoc cref="JsonSerializer.Serialize{TValue}(TValue,System.Text.Json.JsonSerializerOptions?)"/>
    public static string ToJson(this object source, JsonSerializerOptions? options = default)
    {
        return JsonSerializer.Serialize(source, options);
    }

    /// <inheritdoc cref="JsonSerializer.Deserialize{TValue}(System.String,System.Text.Json.JsonSerializerOptions?)"/>
    public static T? FromJson<T>(this string json, JsonSerializerOptions? options = default)
    {
        return JsonSerializer.Deserialize<T>(json, options);
    }
}
