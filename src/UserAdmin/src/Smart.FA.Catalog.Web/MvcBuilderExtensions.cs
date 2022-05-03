using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Smart.FA.Catalog.Web;
public static class MvcBuilderExtensions
{
    public static IMvcBuilder ConfigureRazorPagesOptions(this IMvcBuilder mvcBuilder)
    {
        mvcBuilder.AddRazorPagesOptions(options =>
        {
            options.Conventions.AddPageConventionsAuthorization();
        });

        return mvcBuilder;
    }

    private static void AddPageConventionsAuthorization(this PageConventionCollection conventions)
    {
        conventions.AuthorizeFolder("/Admin", Policies.List.AtLeastOneValidUserChartRevisionApproval);
        conventions.AuthorizeFolder("/SuperUser", Policies.List.MustBeSuperUser);
    }
}
