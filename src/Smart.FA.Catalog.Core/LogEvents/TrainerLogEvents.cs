using Microsoft.Extensions.Logging;

namespace Core.LogEvents;

public static partial class LogEventIds
{
    public static EventId TrainerCreated = new(20004, nameof(TrainerCreated));
    public static EventId TrainerUpdated = new(20005, nameof(TrainerUpdated));
    public static EventId TrainerDeleted = new(20006, nameof(TrainerDeleted));
}
