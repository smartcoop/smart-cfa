namespace Smart.FA.Catalog.Core.Domain.Factories;

public static class UserChartFactory
{
    public static UserChart CreateDefault() => new UserChart("default", "V1", null, null);
}
