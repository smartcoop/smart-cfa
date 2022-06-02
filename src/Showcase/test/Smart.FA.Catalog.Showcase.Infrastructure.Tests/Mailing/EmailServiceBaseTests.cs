using System.Threading.Tasks;
using FluentAssertions;
using FluentEmail.Core;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Smart.FA.Catalog.Showcase.Domain.Common.Enums;
using Smart.FA.Catalog.Showcase.Infrastructure.Tests.Mailing.Common;
using Xunit;

namespace Smart.FA.Catalog.Showcase.Infrastructure.Tests.Mailing;

public class EmailServiceBaseTests
{
    [Fact]
    public async Task SendingTwoMails_BeforeRateLimitTimeFrameExpire_ShouldReturnTooManyRequests()
    {
        // Arrange
        var memoryCache = new MemoryCache(Options.Create(new MemoryCacheOptions()));
        var emailService = new TestInquiryEmailService(
            logger: Mock.Of<ILogger<TestInquiryEmailService>>(),
            fluentEmail: Mock.Of<IFluentEmail>(),
            memoryCache: memoryCache,
            rateLimitInSeconds: 2);

        // Act
        await emailService.SendEmailAsync(new TestEmailRequest());
        var result = await emailService.SendEmailAsync(new TestEmailRequest());

        // Assert
        result.Should().Be(InquirySendEmailResult.TooManyRequests);
    }

    [Fact]
    public async Task SendingTwoMails_WithHigherDelayBetweenThanRateLimitTimeFrame_ShouldReturnSuccess()
    {
        // Arrange
        var memoryCache = new MemoryCache(Options.Create(new MemoryCacheOptions()));
        var emailService = new TestInquiryEmailService(
            logger: Mock.Of<ILogger<TestInquiryEmailService>>(),
            fluentEmail: Mock.Of<IFluentEmail>(),
            memoryCache: memoryCache,
            rateLimitInSeconds: 1);

        // Act
        await emailService.SendEmailAsync(new TestEmailRequest());
        await Task.Delay(1000);
        var result = await emailService.SendEmailAsync(new TestEmailRequest());

        // Assert
        result.Should().Be(InquirySendEmailResult.Success);
    }
}
