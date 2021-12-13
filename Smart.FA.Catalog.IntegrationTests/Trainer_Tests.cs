using System.Threading.Tasks;
using Core.Domain;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Smart.FA.Catalog.IntegrationTests.Base;
using Xunit;

namespace Smart.FA.Catalog.Application.Tests.UseCases;

[Collection("Integration test collection")]
public class TrainerTests : IntegrationTestBase
{

    public TrainerTests()
    {

    }

    [Theory]
    [InlineData("Victor")]
    public async Task Can_get_trainers(string trainerName)
    {
        using var context = GivenTrainingContext();

        Trainer? trainer = await context.Trainer.FirstOrDefaultAsync(trainer => trainer.Name == trainerName);

        trainer.Should().NotBeNull();
    }
}
