using Microsoft.Extensions.Logging;

namespace Smart.FA.Catalog.Core.LogEvents;

public partial class LogEventIds
{
    public static EventId UserChartRevisionCreated = new(30001, nameof(UserChartRevisionCreated));
}
