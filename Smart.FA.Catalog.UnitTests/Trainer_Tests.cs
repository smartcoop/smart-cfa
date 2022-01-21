using System;
using System.Linq;
using AutoFixture;
using Core.Domain;
using Core.Domain.Enumerations;
using FluentAssertions;
using Smart.FA.Catalog.Tests.Common;
using Xunit;

namespace Smart.FA.Catalog.UnitTests;

public class TrainerTests
{
    private readonly TrainerFactory _trainerFactory = new();
    private readonly TrainingFactory _trainingFactory = new();
    private readonly Fixture _fixture = new();

    [Fact]
    public void HasInitiallyNoTraining()
    {
        var trainer = _trainerFactory.CreateClean();

        trainer.Enrollments.Should().BeEmpty();
    }

    [Theory]
    [InlineData("Victor", "van Duynen", "Hello my name is Victor van Duynen", "EN")]
    public void CanCreateWithValidDescription(string firstName, string lastName, string description, string language)
    {
        var action = () => new Trainer(Name.Create(firstName, lastName).Value, TrainerIdentity.Create(_fixture.Create<string>(), ApplicationType.Account).Value, description, Language.Create(language).Value);

        action.Should().NotThrow<Exception>();
    }

    [Theory]
    [InlineData("Victor", "van Duynen", "Hello my name is Victor van Duynen", "EN")]
    public void CanUpdateWithValidDescription(string firstName, string lastName, string description, string language)
    {
       var trainer = new Trainer(Name.Create(firstName, lastName).Value, TrainerIdentity.Create(_fixture.Create<string>(), ApplicationType.Account).Value, description, Language.Create(language).Value);
       string updatedDescription = description + "!";

       var action = () => trainer.UpdateDescription(updatedDescription);

        action.Should().NotThrow<Exception>();
        trainer.Description.Should().BeEquivalentTo(updatedDescription);
    }

    [Fact]
    public void CantCreateWithInvalidDescription()
    {
        var action = () => new Trainer(Name.Create(_fixture.Create<string>(), _fixture.Create<string>()).Value, TrainerIdentity.Create(_fixture.Create<string>(), ApplicationType.Account).Value,string.Concat(Enumerable.Repeat('a', 2001)),  Language.Create(_fixture.Create<string>().Substring(0,2)).Value);

        action.Should().Throw<Exception>();
    }

    [Fact]
    public void CantUpdateWithInvalidDescription()
    {
        string description = "Hello my name is Victor van Duynen";
        var trainer = new Trainer(Name.Create("Victor", "van Duynen").Value, TrainerIdentity.Create(_fixture.Create<string>(), ApplicationType.Account).Value, description, Language.Create("EN").Value );

        var action = () => trainer.UpdateDescription(string.Concat(Enumerable.Repeat('a', 2000)));

        action.Should().Throw<Exception>();
        trainer.Description.Should().BeEquivalentTo(description);
    }
    [Fact]
    public void CanEnrollInTraining()
    {
        var otherTrainer = _trainerFactory.CreateClean();
        var training = _trainingFactory.Create(otherTrainer);
        var trainerToEnroll = _trainerFactory.CreateClean();

        trainerToEnroll.EnrollIn(training);

        trainerToEnroll.Enrollments.Select(enrollment => enrollment.Training).Should().Contain(training);
    }

    [Fact]
    public void CanDisenrollFromTraining()
    {
        var training = _trainingFactory.CreateClean();
        var trainerToEnroll = _trainerFactory.CreateClean();
        trainerToEnroll.EnrollIn(training);

        trainerToEnroll.DisenrollFrom(training);

        trainerToEnroll.Enrollments.Should().BeEmpty();
    }

}
