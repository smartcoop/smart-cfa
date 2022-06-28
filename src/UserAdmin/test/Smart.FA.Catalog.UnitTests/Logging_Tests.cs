using System.Linq;
using Microsoft.Extensions.Logging;
using Xunit;
using FluentAssertions;
using LogEventIds = Smart.FA.Catalog.Core.LogEvents.LogEventIds;

namespace Smart.FA.Catalog.UnitTests;

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
