namespace Smart.FA.Catalog.UserAdmin.Web.Authorization.Policy;

public static class Policies
{
    public const string AtLeastOneValidUserChartRevisionApproval = "AtLeastOneValidUserChartRevisionApproval";

    public const string MustBeSuperUser = nameof(MustBeSuperUser);

    public const string MustBeSuperUserOrTrainingCreator = nameof(MustBeSuperUserOrTrainingCreator);
}
