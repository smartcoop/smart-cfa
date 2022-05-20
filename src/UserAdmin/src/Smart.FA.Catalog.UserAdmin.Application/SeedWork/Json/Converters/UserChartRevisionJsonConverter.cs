using System.Text.Json;
using System.Text.Json.Serialization;
using Smart.FA.Catalog.UserAdmin.Domain.Domain;

namespace Smart.FA.Catalog.UserAdmin.Application.SeedWork.Json.Converters;

public class UserChartRevisionJsonConverter : JsonConverter<UserChartRevision>
{
    public override UserChartRevision? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return JsonSerializer.Deserialize(ref reader, typeToConvert, options) as UserChartRevision;
    }

    public override void Write(Utf8JsonWriter writer, UserChartRevision userChartRevision, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(userChartRevision.Id);
    }
}
