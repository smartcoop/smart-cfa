using System.Threading;
using System.Threading.Tasks;
using Application.UseCases.Queries;
using AutoFixture;
using Core.Domain.Enumerations;
using FluentAssertions;
using Infrastructure.Services;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;
using Smart.FA.Catalog.IntegrationTests.Base;

namespace Smart.FA.Catalog.IntegrationTests.TrainingContext;
[Collection("Integration test collection")]
public class UserTests: IntegrationTestBase
{
    private readonly Fixture _fixture = new();

    [Fact]
    public async Task CanGet()
    {
        var logger = Substitute.For<ILogger<GetTrainerFromUserAppQueryHandler>>();
        await using var context = GivenTrainingContext();
        var userStrategyResolver = Substitute.For<UserStrategyResolver>("");
        var handler = Substitute.For<GetTrainerFromUserAppQueryHandler>(logger, context, userStrategyResolver );
        var request = new GetTrainerFromUserAppRequest {ApplicationType = ApplicationType.Account, UserId = _fixture.Create<string>()};

        var user = await handler.Handle(request, CancellationToken.None);

        user.Should().NotBeNull();
        user.User.UserId.Should().Equals(request.UserId);
    }
}
