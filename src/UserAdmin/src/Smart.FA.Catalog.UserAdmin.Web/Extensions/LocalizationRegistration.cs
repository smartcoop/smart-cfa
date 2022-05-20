using System.Globalization;
using Microsoft.AspNetCore.Localization;

namespace Smart.FA.Catalog.UserAdmin.Web.Extensions;

public static class LocalizationRegistration
{
    public static IServiceCollection AddCatalogLocalization(this IServiceCollection services)
    {
        return services
            .AddLocalization()
            .Configure<RequestLocalizationOptions>(
                options =>
                {
                    //TODO uncomment specific language once their localization becomes available
                    var supportedCultures = new List<CultureInfo>
                    {
                        // new("en"),
                        // new("nl"),
                        new("fr")
                    };

                    // Requirements is to have french has default language.
                    options.DefaultRequestCulture = new RequestCulture("fr", "fr");

                    // Formatting numbers, dates, etc.
                    options.SupportedCultures = supportedCultures;

                    // UI strings that we have localized.
                    options.SupportedUICultures = supportedCultures;
                }
            );
    }
}
