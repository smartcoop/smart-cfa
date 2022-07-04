using Microsoft.Extensions.Logging;

namespace Smart.FA.Catalog.Core.LogEvents;

public static partial class LogEventIds
{
    public static EventId TrainerCreated = new(20004, nameof(TrainerCreated));
    public static EventId TrainerUpdated = new(20005, nameof(TrainerUpdated));
    public static EventId TrainerDeleted = new(20006, nameof(TrainerDeleted));
    public static EventId TrainerProfileImageUpdated = new(20007, nameof(TrainerProfileImageUpdated));
    public static EventId TrainerNotFound = new(20008, nameof(TrainerNotFound));
}
