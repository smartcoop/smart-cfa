using Microsoft.Extensions.Logging;

namespace Core.LogEvents;

public static partial class LogEventIds
{
    public static EventId TrainingCreated = new(10001, nameof(TrainingCreated));
    public static EventId TrainingUpdated = new(10002, nameof(TrainingUpdated));
    public static EventId TrainingDeleted = new(10003, nameof(TrainingDeleted));
}
