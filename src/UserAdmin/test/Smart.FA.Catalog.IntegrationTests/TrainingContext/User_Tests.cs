using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Smart.FA.Catalog.Application.UseCases.Queries;
using Smart.FA.Catalog.Core.Domain.User.Enumerations;
using Smart.FA.Catalog.Infrastructure.Services;
using Xunit;
using Smart.FA.Catalog.IntegrationTests.Base;

namespace Smart.FA.Catalog.IntegrationTests.TrainingContext;
[Collection("Integration test collection")]
public class UserTests: IntegrationTestBase
{
    private readonly Fixture _fixture = new();

    [Fact]
    public async Task GetTrainerFromUserAppRequest()
    {
        var logger = Substitute.For<ILogger<GetTrainerFromUserAppQueryHandler>>();
        await using var context = GivenCatalogContext();
        var userStrategyResolver = Substitute.For<UserStrategyResolver>("");
        var handler = Substitute.For<GetTrainerFromUserAppQueryHandler>(logger, context, userStrategyResolver );
        var request = new GetTrainerFromUserAppRequest(applicationType: ApplicationType.Account, userId: _fixture.Create<string>());

        var trainer = await handler.Handle(request, CancellationToken.None);

        trainer.Should().NotBeNull();
        trainer.User.UserId.Should().Equals(request.UserId);
    }
}
