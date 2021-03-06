using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Smart.FA.Catalog.Web.Authorization.Policy;
using Smart.FA.Catalog.Web.Common.ModelBinding.Binders;
using Smart.FA.Catalog.Web.Filters;

namespace Smart.FA.Catalog.Web.Extensions;

public static class MvcBuilderExtensions
{
    public static IMvcBuilder ConfigureRazorPagesOptions(this IMvcBuilder mvcBuilder)
    {
        mvcBuilder
            .AddRazorPagesOptions(options =>  options.Conventions.AddPageConventionsAuthorization())
            .AddMvcOptions(options => options.Filters.Add<SerializeModelStateFilter>());
        return mvcBuilder;
    }

    public static IMvcBuilder ConfigureMvcOptions(this IMvcBuilder mvcBuilder)
    {
        return mvcBuilder.AddMvcOptions(options =>
        {
            options.ModelBinderProviders.Insert(0, new SanitizeBinderProvider());
        });
    }

    private static void AddPageConventionsAuthorization(this PageConventionCollection conventions)
    {
        // Single pages
        conventions.AuthorizePage("/Admin/Trainings/Update/Index", Policies.MustBeSuperUserOrTrainingCreator);

        // Folders
        conventions.AuthorizeFolder("/Admin", Policies.MustNotBeBlackListed);
        conventions.AuthorizeFolder("/SuperUser", Policies.MustNotBeBlackListed);
        conventions.AuthorizeFolder("/Admin", Policies.AtLeastOneValidUserChartRevisionApproval);
        conventions.AuthorizeFolder("/SuperUser", Policies.MustBeSuperUser);
    }
}
