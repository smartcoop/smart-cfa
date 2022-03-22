using Microsoft.Extensions.Logging;

namespace Smart.FA.Catalog.Core.LogEvents;

public static partial class LogEventIds
{
    public static EventId DuplicateEventId = new(00001, nameof(DuplicateEventId));
    public static EventId ErrorEventId = new(00002, nameof(ErrorEventId));
}
