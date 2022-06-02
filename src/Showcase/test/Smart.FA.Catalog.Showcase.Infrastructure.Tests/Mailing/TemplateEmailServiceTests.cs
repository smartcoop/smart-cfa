using System.Linq;
using System.Reflection;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Smart.FA.Catalog.Showcase.Domain.Common.Options;
using Smart.FA.Catalog.Showcase.Infrastructure.Mailing.Inquiry;
using Smart.FA.Catalog.Showcase.Infrastructure.Mailing.Inquiry.SmartLearningTeam;
using Smart.FA.Catalog.Showcase.Infrastructure.Mailing.Inquiry.Trainer;
using Xunit;

namespace Smart.FA.Catalog.Showcase.Infrastructure.Tests.Mailing;
public class TemplateEmailServiceTests
{

    private readonly Assembly _infrastructureAssembly;

    private string[] GetManifestResourceNames() => _infrastructureAssembly.GetManifestResourceNames();

    private bool TemplateExists(string templatePath) => GetManifestResourceNames().Contains(templatePath);

    public TemplateEmailServiceTests()
    {
        _infrastructureAssembly = typeof(InquiryEmailServiceBase).Assembly;
    }

    [Fact]
    public void SmartLearningTeamInquiryTemplateProperty_ShouldBeAnExistingEmbeddedResource()
    {
        // Arrange
        var inquiryEmailService = new SmartLearningTeamInquiryEmailService(default,
            default,
            default,
            Options.Create(new FluentEmailOptions()),
            Options.Create(new InquiryOptions()));

        // Act
        var templateExists = TemplateExists(inquiryEmailService.Template);

        // Assert
        templateExists.Should().BeTrue();
    }

    [Fact]
    public void TrainerInquiryTemplateIs_ShouldBeAnExistingEmbeddedResource()
    {
        // Arrange
        var trainerInquirySendEmailService = new TrainerInquirySendEmailService(
            logger: default,
            serviceProvider: default,
            fluentEmail: default,
            memoryCache: default,
            fluentEmailOptions: Options.Create(new FluentEmailOptions()),
            inquiryOptions: Options.Create(new InquiryOptions()));

        // Act
        var templateExists = TemplateExists(trainerInquirySendEmailService.Template);

        // Assert
        templateExists.Should().BeTrue();
    }
}
