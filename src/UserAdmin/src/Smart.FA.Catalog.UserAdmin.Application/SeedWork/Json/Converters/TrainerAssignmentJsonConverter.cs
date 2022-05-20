using System.Text.Json;
using System.Text.Json.Serialization;
using Smart.FA.Catalog.UserAdmin.Domain.Domain;

namespace Smart.FA.Catalog.UserAdmin.Application.SeedWork.Json.Converters;

public class TrainerAssignmentJsonConverter : JsonConverter<TrainerAssignment>
{
    public override TrainerAssignment? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return JsonSerializer.Deserialize(ref reader, typeToConvert, options) as TrainerAssignment;
    }

    public override void Write(Utf8JsonWriter writer, TrainerAssignment trainerAssignment, JsonSerializerOptions options)
    {
        writer.WriteStringValue($"[TrainerId: {trainerAssignment.TrainerId}, TrainingId: {trainerAssignment.TrainingId}]");
    }
}
