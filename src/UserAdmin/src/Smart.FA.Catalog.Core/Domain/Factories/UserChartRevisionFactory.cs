namespace Smart.FA.Catalog.Core.Domain.Factories;

public static class UserChartRevisionFactory
{
    public static UserChartRevision CreateDefault() => new ("default", "V1", DateTime.UtcNow, null);
}
