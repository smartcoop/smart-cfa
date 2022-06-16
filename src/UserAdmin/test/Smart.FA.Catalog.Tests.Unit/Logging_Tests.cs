using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Xunit;
using LogEventIds = Smart.FA.Catalog.Core.LogEvents.LogEventIds;

namespace Smart.FA.Catalog.Tests.Unit;

public class LoggingTests
{
    [Fact]
    public static void CheckDuplicateLogEvents()
    {
        var eventType = typeof(LogEventIds);

        var eventFields = eventType.GetFields().Select(fieldInfo => (EventId?)fieldInfo.GetValue(null) ?? 0);

        eventFields.Should().OnlyHaveUniqueItems();
    }
}
