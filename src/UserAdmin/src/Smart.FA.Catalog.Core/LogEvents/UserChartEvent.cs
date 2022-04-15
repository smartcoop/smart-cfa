using Microsoft.Extensions.Logging;

namespace Smart.FA.Catalog.Core.LogEvents;

public partial class LogEventIds
{
    public static EventId UserChartCreated = new(30001, nameof(UserChartCreated));
}
