using System.Text.Json;
using System.Text.Json.Serialization;
using Smart.FA.Catalog.UserAdmin.Domain.Domain;

namespace Smart.FA.Catalog.UserAdmin.Application.SeedWork.Json.Converters;

public class TrainerJsonConverter : JsonConverter<Trainer>
{
    public override Trainer? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return JsonSerializer.Deserialize(ref reader, typeToConvert, options) as Trainer;
    }

    public override void Write(Utf8JsonWriter writer, Trainer trainer, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(trainer.Id);
    }
}
