using System;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using Smart.FA.Catalog.Core.Domain;
using Smart.FA.Catalog.Core.Domain.User.Enumerations;
using Smart.FA.Catalog.Core.Domain.ValueObjects;
using Smart.FA.Catalog.Tests.Common.Extensions;
using Smart.FA.Catalog.Tests.Common.Factories;
using Smart.FA.Catalog.Tests.Common;
using Xunit;

namespace Smart.FA.Catalog.UnitTests;

public class TrainerTests
{
    private readonly Fixture _fixture = new();

    [Fact]
    public void HasInitiallyNoTraining()
    {
        var trainer = MockedTrainerFactory.CreateClean();

        trainer.Assignments.Should().BeEmpty();
    }

    [Theory]
    [JsonFileData("data.json", "Trainer")]
    public void CanCreateTrainerWithValidDescription(string firstName, string lastName, string title, string description, string language, params object[] extra)
    {
        var action = () => new Trainer(Name.Create(firstName, lastName).Value, TrainerIdentity.Create(_fixture.Create<string>(), ApplicationType.Account).Value, title, description,
            Language.Create(language).Value);

        action.Should().NotThrow<Exception>();
    }

    [Theory]
    [JsonFileData("data.json", "Trainer")]
    public void CanUpdateTrainerWithValidDescription(string firstName, string lastName, string title, string description, string language)
    {
        var trainer = new Trainer(Name.Create(firstName, lastName).Value, TrainerIdentity.Create(_fixture.Create<string>(), ApplicationType.Account).Value, title, description,
            Language.Create(language).Value);
        string updatedDescription = description + "!";

        var action = () => trainer.UpdateBiography(updatedDescription);

        action.Should().NotThrow<Exception>();
        trainer.Biography.Should().BeEquivalentTo(updatedDescription);
    }

    [Fact]
    public void CantCreateTrainerWithInvalidDescription()
    {
        var action = () => new Trainer(Name.Create(_fixture.Create<string>(), _fixture.Create<string>()).Value, TrainerIdentity.Create(_fixture.Create<string>(), ApplicationType.Account).Value,
            _fixture.Create<string>(), string.Concat(Enumerable.Repeat('a', 2001)), Language.Create(_fixture.Create<string>().Substring(0, 2)).Value);

        action.Should().Throw<Exception>();
    }

    [Fact]
    public void CantUpdateTrainerWithInvalidDescription()
    {
        string description = _fixture.Create<string>();
        var trainer = new Trainer(Name.Create(_fixture.Create<string>(), _fixture.Create<string>()).Value,
            TrainerIdentity.Create(_fixture.Create<string>(), ApplicationType.Account).Value, _fixture.Create<string>(), description, Language.Create(_fixture.Create<string>()[..2]).Value);

        var action = () => trainer.UpdateBiography(string.Concat(Enumerable.Repeat('a', 2000)));

        action.Should().Throw<Exception>();
        trainer.Biography.Should().BeEquivalentTo(description);
    }

    [Fact]
    public void CanAssignInTraining()
    {
        var otherTrainer = MockedTrainerFactory.CreateClean();
        var training = MockedTrainingFactory.Create(otherTrainer);
        var trainerToAssign = MockedTrainerFactory.CreateClean();

        trainerToAssign.AssignTo(training);

        trainerToAssign.Assignments.Select(assignment => assignment.Training).Should().Contain(training);
    }

    [Fact]
    public void CanUnAssignTrainerFromTraining()
    {
        var training = MockedTrainingFactory.CreateClean();
        var trainerToUnAssign = MockedTrainerFactory.CreateClean();
        trainerToUnAssign.AssignTo(training);

        trainerToUnAssign.UnAssignFrom(training);

        trainerToUnAssign.Assignments.Should().BeEmpty();
    }
}
