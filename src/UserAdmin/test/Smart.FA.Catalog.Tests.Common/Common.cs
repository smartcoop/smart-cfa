using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Smart.FA.Catalog.Infrastructure.Services;

namespace Smart.FA.Catalog.Tests.Common;

public static class LoggerFactory
{
    public static ILogger<T> Create<T>()
    {
        return Substitute.For<ILogger<T>>();
    }
}

public static class DomainEventPublisherFactory
{
    public static DomainEventPublisher Create()
    {
        return Substitute.For<DomainEventPublisher>(LoggerFactory.Create<DomainEventPublisher>(), Substitute.For<IPublisher>());
    }
}
