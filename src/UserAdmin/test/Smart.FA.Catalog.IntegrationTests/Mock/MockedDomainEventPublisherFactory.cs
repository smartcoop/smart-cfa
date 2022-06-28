using MediatR;
using NSubstitute;
using Smart.FA.Catalog.Infrastructure.Services;
using Smart.FA.Catalog.Tests.Common.Factories;

namespace Smart.FA.Catalog.IntegrationTests.Mock;

public static class MockedDomainEventPublisherFactory
{
    public static DomainEventPublisher Create()
    {
        return Substitute.For<DomainEventPublisher>(MockedLoggerFactory.Create<DomainEventPublisher>(), Substitute.For<IPublisher>());
    }
}
