namespace Smart.FA.Catalog.Web.Authorization.Policy;

public static class Policies
{
    public const string AtLeastOneValidUserChartRevisionApproval = "AtLeastOneValidUserChartRevisionApproval";

    public const string MustBeSuperUser = nameof(MustBeSuperUser);
}
