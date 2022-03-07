using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Core.Domain;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Smart.FA.Catalog.IntegrationTests.Base;
using Smart.FA.Catalog.Tests.Common;
using Xunit;

namespace Smart.FA.Catalog.IntegrationTests.TrainingContext;

[Collection("Integration test collection")]
public class TrainerTests : IntegrationTestBase
{
    private readonly TrainerFactory _trainerFactory = new();
    private readonly UserFactory _userFactory = new();

    [Theory]
    [InlineData("Victor", "vD")]
    public async Task CanCreateTrainer(string firstName, string lastName)
    {
        await using var context = GivenTrainingContext();
        var trainer = _trainerFactory.Create(firstName, lastName);

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
    [InlineData("Victor", "vD")]
    public async Task CanChangeTrainerName(string firstName, string lastName)
    {
        await using var context = GivenTrainingContext();
        var trainer = _trainerFactory.Create(firstName, lastName);
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
        await using var context = GivenTrainingContext();
        var user = _userFactory.CreateClean();
        var trainer = _trainerFactory.CreateFromUser(user);

        context.Trainers.Add(trainer);
        await context.SaveChangesAsync();

        trainer.Should().NotBeNull();
        trainer!.Name.FirstName.Should().NotBeEmpty();
        trainer.Name.LastName.Should().NotBeEmpty();
        trainer.DefaultLanguage.Value.Should().NotBeEmpty();
    }
}
