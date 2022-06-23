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
        // Single pages
        conventions.AuthorizePage("/Admin/Trainings/Update/Index", Policies.MustBeSuperUserOrTrainingCreator);

        // Folders
        conventions.AuthorizeFolder("/Admin", Policies.AtLeastOneValidUserChartRevisionApproval);
        conventions.AuthorizeFolder("/SuperUser", Policies.MustBeSuperUser);
        conventions.AuthorizeFolder("/Admin/Trainings", Policies.MustBeShareholder);
        conventions.AuthorizeFolder("/Admin/Trainers", Policies.MustBeShareholder);
    }
}
