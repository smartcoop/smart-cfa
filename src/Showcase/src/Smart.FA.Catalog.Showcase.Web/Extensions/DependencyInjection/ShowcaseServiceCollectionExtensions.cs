using Microsoft.EntityFrameworkCore;
using Smart.Design.Razor.Extensions;
using Smart.FA.Catalog.Showcase.Domain.Common.Options;
using Smart.FA.Catalog.Showcase.Infrastructure.Data;
using Smart.FA.Catalog.Showcase.Infrastructure.Mailing.Inquiry.SmartLearningTeam;
using Smart.FA.Catalog.Showcase.Infrastructure.Mailing.Inquiry.Trainer;
using Smart.FA.Catalog.Showcase.Web.Options;
using Smart.FA.Catalog.Showcase.Web.Services.Trainer;
using Smart.FA.Catalog.Showcase.Web.Services.Training;

namespace Smart.Extensions.DependencyInjection;

public static class ShowcaseServiceCollectionExtensions
{
    public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
    {
        return services
            .AddTransientServices(webHostEnvironment)
            .AddFluentEmail(configuration)
            .AddShowcaseLocalization()
            .AddEfCore(configuration)
            .AddMemoryCache()
            .AddSmartDesign()
            .AddHttpContextAccessor();
    }

    private static IServiceCollection AddEfCore(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddDbContext<CatalogShowcaseContext>(dbContextOptionsBuilder =>
            dbContextOptionsBuilder.UseSqlServer(configuration.GetConnectionString("Catalog")));
    }

    private static IServiceCollection AddTransientServices(this IServiceCollection services, IWebHostEnvironment webHostEnvironment)
    {
        services
            .AddTransient<ITrainingService, TrainingService>()
            .AddTransient<ITrainerService, TrainerService>()
            .AddTransient<ISmartLearningInquiryEmailService, SmartLearningTeamInquiryEmailService>();

        if (webHostEnvironment.IsProduction())
        {
            services.AddTransient<ITrainerInquirySendEmailService, TrainerInquirySendEmailService>();
        }
        else
        {
            services.AddTransient<ITrainerInquirySendEmailService, InquiryTestEmailService>();
        }

        return services;
    }

    public static IServiceCollection ConfigureShowcaseOptions(this IServiceCollection services, IConfiguration configuration)
    {
        return services.Configure<MinIOOptions>(configuration.GetSection(MinIOOptions.SectionName))
            .Configure<FluentEmailOptions>(configuration.GetSection(FluentEmailOptions.SectionName))
            .Configure<InquiryOptions>(configuration.GetSection(InquiryOptions.SectionName))
            .Configure<TestInquiryOptions>(configuration.GetSection(TestInquiryOptions.SectionName));
    }
}
