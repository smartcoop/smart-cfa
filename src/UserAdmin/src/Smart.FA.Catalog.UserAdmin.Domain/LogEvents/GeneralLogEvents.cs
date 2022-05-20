using Microsoft.Extensions.Logging;

namespace Smart.FA.Catalog.UserAdmin.Domain.LogEvents;

public static partial class LogEventIds
{
    public static EventId DuplicateEventId = new(00001, nameof(DuplicateEventId));
    public static EventId ErrorEventId = new(00002, nameof(ErrorEventId));

    public static readonly EventId DomainEventDispatch = new(0_003, nameof(DomainEventDispatch));
}
