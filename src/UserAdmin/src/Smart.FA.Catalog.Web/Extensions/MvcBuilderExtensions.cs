using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Smart.FA.Catalog.Web.Authorization.Policy;

namespace Smart.FA.Catalog.Web.Extensions;

public static class MvcBuilderExtensions
{
    public static IMvcBuilder ConfigureRazorPagesOptions(this IMvcBuilder mvcBuilder)
    {
        // Sets all routes and query strings in lowercase
        mvcBuilder.Services.Configure<RouteOptions>(options =>
        {
            options.LowercaseUrls = true;
            options.LowercaseQueryStrings = true;
        });

        mvcBuilder.AddRazorPagesOptions(options =>
        {
            options.Conventions.AddPageConventionsAuthorization();
        });

        return mvcBuilder;
    }

    private static void AddPageConventionsAuthorization(this PageConventionCollection conventions)
    {
        // Single pages
        conventions.AuthorizePage("/Admin/Trainings/Update/Index", Policies.MustBeSuperUserOrTrainingCreator);

        // Folders
        conventions.AuthorizeFolder("/Admin", Policies.AtLeastOneValidUserChartRevisionApproval);
        conventions.AuthorizeFolder("/SuperUser", Policies.MustBeSuperUser);
    }
}
