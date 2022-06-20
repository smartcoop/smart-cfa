namespace Smart.FA.Catalog.Web.Authorization.Policy;

public static class Policies
{
    public const string AtLeastOneValidUserChartRevisionApproval = nameof(AtLeastOneValidUserChartRevisionApproval);

    public const string MustBeSuperUser = nameof(MustBeSuperUser);

    public const string MustBeSocialMember = nameof(MustBeSocialMember);

    public const string MustBeSuperUserOrTrainingCreator = nameof(MustBeSuperUserOrTrainingCreator);
}
