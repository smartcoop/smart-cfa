using FluentEmail.MailKitSmtp;
using Smart.FA.Catalog.Showcase.Domain.Common.Options;

namespace Smart.Extensions.DependencyInjection;

public static class FluentEmailServiceCollectionExtensions
{
    public static IServiceCollection AddFluentEmail(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.GetSection(FluentEmailOptions.SectionName).Get<FluentEmailOptions>();

        return services
            .AddFluentEmail(options.DefaultSender)
            .AddMailKitSender(new SmtpClientOptions()
            {
                User = options.User,
                Password = options.Password,
                Port = options.Port,
                Server = options.Server,
                UseSsl = options.UseSsl,
                RequiresAuthentication = options.RequiresAuthentication
            })
            .AddRazorRenderer()
            .Services;
    }
}
