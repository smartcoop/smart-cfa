using AutoFixture;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Smart.FA.Catalog.Core.Domain;
using Smart.FA.Catalog.Core.Domain.Models;
using Smart.FA.Catalog.Core.Domain.User.Enumerations;
using Smart.FA.Catalog.Core.Domain.ValueObjects;
using Smart.FA.Catalog.Core.Services;

namespace Smart.FA.Catalog.Tests.Common.Factories;

public static class MockedLoggerFactory
{
    public static ILogger<T> Create<T>()
    {
        return Substitute.For<ILogger<T>>();
    }
}

public static class MockedUserIdentityFactory
{
    private static Fixture _fixture = new();

    public static IUserIdentity Create()
    {
        return new MockedUserIdentity();
    }

    class MockedUserIdentity : IUserIdentity
    {
        public int Id { get; }

        public CustomIdentity Identity { get; } =
            new(
                new(
                    Name.Create(_fixture.Create<string>(),
                        _fixture.Create<string>()).Value,
                    TrainerIdentity.Create(_fixture.Create<short>().ToString(),
                        ApplicationType.Default).Value,
                    _fixture.Create<string>(),
                    _fixture.Create<string>(),
                    Language.Create(_fixture.Create<string>()[..2]).Value,
                    $"{Guid.NewGuid()}@gmail.com"
                )
            );

        public Trainer CurrentTrainer { get; }

        public bool IsSuperUser { get; }
    }
}


public static class MockedUserChartRevisionFactory
{
    private static Fixture _fixture = new();
    public static UserChartRevision Create()
    {
        return Substitute.For<UserChartRevision>(_fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<DateTime>(), _fixture.Create<DateTime>());
    }
}
