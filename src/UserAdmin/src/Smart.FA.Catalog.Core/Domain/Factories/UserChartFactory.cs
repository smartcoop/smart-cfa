namespace Smart.FA.Catalog.Core.Domain.Factories;

public static class UserChartFactory
{
    public static UserChartRevision CreateDefault() => new ("default", "V1", DateTime.UtcNow, null);
}
