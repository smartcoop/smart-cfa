using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Smart.FA.Catalog.Web.Authorization.Policy;

namespace Smart.FA.Catalog.Web.Extensions;

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
        conventions.AuthorizeFolder("/Admin", Policies.AtLeastOneValidUserChartRevisionApproval);
        conventions.AuthorizeFolder("/SuperUser", Policies.MustBeSuperUser);
    }
}
