using System.Globalization;
using Microsoft.AspNetCore.Localization;

namespace Smart.Extensions.DependencyInjection;

public static class LocalizationServiceCollectionExtensions
{
    public static IServiceCollection AddShowcaseLocalization(this IServiceCollection services)
    {
        return services
            .AddLocalization()
            .Configure<RequestLocalizationOptions>(
                options =>
                {
                    var supportedCultures = new List<CultureInfo>
                    {
                        new("en"),
                        new("nl"),
                        new("fr")
                    };

                    // Requirements are to have french has default language.
                    options.DefaultRequestCulture = new RequestCulture("fr", "fr");

                    // Formatting numbers, dates, etc.
                    options.SupportedCultures = supportedCultures;

                    // UI strings that we have localized.
                    options.SupportedUICultures = supportedCultures;
                }
            );
    }
}
