using System.Text.Json;
using System.Text.Json.Serialization;
using Smart.FA.Catalog.Core.Domain.Authorization;

namespace Smart.FA.Catalog.Application.SeedWork.Json.Converters;

public class SuperUserJsonConverter : JsonConverter<SuperUser>
{
    public override SuperUser? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return JsonSerializer.Deserialize(ref reader, typeToConvert, options) as SuperUser;
    }

    public override void Write(Utf8JsonWriter writer, SuperUser superUser, JsonSerializerOptions options)
    {
        writer.WriteStringValue(superUser.UserId);
    }
}
