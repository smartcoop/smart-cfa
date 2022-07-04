using System;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Smart.FA.Catalog.Core.Domain.User.Dto;
using Smart.FA.Catalog.Core.Domain.User.Enumerations;
using Smart.FA.Catalog.Core.Domain.ValueObjects;
using Smart.FA.Catalog.IntegrationTests.Base;
using Smart.FA.Catalog.Tests.Common.Extensions;
using Smart.FA.Catalog.Tests.Common.Factories;
using Xunit;

namespace Smart.FA.Catalog.IntegrationTests.TrainingContext;

[Collection("Integration test collection")]
public class TrainerTests : IntegrationTestBase
{
    private readonly Fixture _fixture = new();

    [Theory]
    [JsonFileData("data.json", "TrainerName")]
    public async Task CanCreateTrainer(string firstName, string lastName)
    {
        await using var context = GivenCatalogContext();
        var trainer = MockedTrainerFactory.Create(firstName, lastName);

        context.Trainers.Attach(trainer);
        await context.SaveChangesAsync();
        var foundTrainer = await context.Trainers.FindAsync(trainer.Id);

        foundTrainer.Should().NotBeNull();
        foundTrainer!.Should().BeSameAs(trainer);
        foundTrainer!.Name.FirstName.Should().NotBeEmpty();
        foundTrainer.Name.LastName.Should().NotBeEmpty();
        foundTrainer.DefaultLanguage.Value.Should().NotBeEmpty();
    }

    [Theory]
    [JsonFileData("data.json", "TrainerName")]
    public async Task CanChangeTrainerName(string firstName, string lastName)
    {
        await using var context = GivenCatalogContext();
        var trainer = MockedTrainerFactory.Create(firstName, lastName);
        context.Trainers.Attach(trainer);
        await context.SaveChangesAsync();
        var newName = Name.Create("Maxime", "P.");

        trainer.Rename(newName.Value);
        var foundTrainer = await context.Trainers.FindAsync(trainer.Id);

        foundTrainer!.Name.Should().BeSameAs(newName.Value);
    }

    [Fact]
    public async Task CanCreateFromUser()
    {
        await using var context = GivenCatalogContext();
        var user = MockedUserFactory.CreateClean();
        var trainer = MockedTrainerFactory.CreateFromUser(user);

        context.Trainers.Add(trainer);
        await context.SaveChangesAsync();
        var trackedTrainer = await context.Trainers.FindAsync(trainer.Id);

        trackedTrainer.Should().NotBeNull();
        trackedTrainer!.Name.Should().BeEquivalentTo(trainer.Name);
        trackedTrainer.DefaultLanguage.Should().BeEquivalentTo(trainer.DefaultLanguage);
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData("1", null)]
    [InlineData(null, 1)]
    public async Task CantCreateFromInvalidUser(string? userId, int? applicationTypeId)
    {
        await using var context = GivenCatalogContext();

        var action = async () =>
        {
            string? applicationName = applicationTypeId is null ? null : ApplicationType.FromValue((int)applicationTypeId).Name;
            var user = new UserDto(userId!, _fixture.Create<string>(), _fixture.Create<string>(), applicationName, "victor@victor.com");
            var trainer = MockedTrainerFactory.CreateFromUser(user);
            context.Trainers.Add(trainer);
            await context.SaveChangesAsync();
        };

        await action.Should().ThrowAsync<Exception>();
    }
}
