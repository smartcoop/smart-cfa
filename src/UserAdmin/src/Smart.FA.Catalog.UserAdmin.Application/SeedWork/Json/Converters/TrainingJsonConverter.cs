using System.Text.Json;
using System.Text.Json.Serialization;
using Smart.FA.Catalog.UserAdmin.Domain.Domain;

namespace Smart.FA.Catalog.UserAdmin.Application.SeedWork.Json.Converters;

public class TrainingJsonConverter : JsonConverter<Training>
{
    public override Training? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return JsonSerializer.Deserialize(ref reader, typeToConvert, options) as Training;
    }

    public override void Write(Utf8JsonWriter writer, Training value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value.Id);
    }
}
