using System.Threading;
using System.Threading.Tasks;
using Application.UseCases.Queries;
using AutoFixture;
using Core.Domain.Enumerations;
using FluentAssertions;
using Infrastructure.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace Smart.FA.Catalog.IntegrationTests.TrainingContext;

public class UserTests
{
    private readonly GetUserAppFromIdQueryHandler _handler;

    public UserTests()
    {
        var fixture = new Fixture();
        var logger = new Mock<ILogger<GetUserAppFromIdQueryHandler>>();
        _handler =
            new GetUserAppFromIdQueryHandler(logger.Object
                , new UserStrategyResolver(fixture.Create<string>()));
    }


    public async Task CanGet()
    {
        var request = new GetUserAppFromIdRequest {ApplicationType = ApplicationType.Account, UserId = "100"};

        GetUserAppFromIdResponse user = await _handler.Handle(request, CancellationToken.None);

        user.Should().NotBeNull();

        user.User.UserId.Should().Equals(request.UserId);
    }
}
