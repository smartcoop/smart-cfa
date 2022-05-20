namespace Smart.FA.Catalog.UserAdmin.Domain.Domain.Factories;

public static class UserChartRevisionFactory
{
    public static UserChartRevision CreateDefault() => new ("default", "V1", DateTime.UtcNow, null);
}
