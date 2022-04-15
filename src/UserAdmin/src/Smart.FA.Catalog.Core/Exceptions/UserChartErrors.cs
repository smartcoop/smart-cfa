namespace Smart.FA.Catalog.Core.Exceptions;

public static partial class Errors
{
    public static class UserChart
    {
        public static Error DontExist=> new("no.user-chart.in.database", $"There are no user charts in the database");
    }
}
