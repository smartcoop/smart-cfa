using Xunit;

namespace Smart.FA.Catalog.UserAdmin.UnitTests;

public class LoggingTests
{
    public LoggingTests()
    {

    }
    [Fact]
    public static void CheckDuplicateLogEvents()
    {
        // var eventType = typeof(LogEventIds);
        //
        // var eventFields = eventType.GetFields().Select( fieldInfo => (EventId?) fieldInfo.GetValue(null) ?? 0);
        //
        // eventFields.Should().OnlyHaveUniqueItems();
    }
}
