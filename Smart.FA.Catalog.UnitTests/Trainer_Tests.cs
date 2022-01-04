using System;
using System.Linq;
using AutoFixture;
using Core.Domain;
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
    public void CanCreateWithValidDescrition(string firstName, string lastName, string description, string language)
    {
        var action = () => new Trainer(firstName, lastName, description, language);

        action.Should().NotThrow<Exception>();
    }

    [Theory]
    [InlineData("Victor", "van Duynen", "Hello my name is Victor van Duynen", "EN")]
    public void CanUpdateWithValidDescrition(string firstName, string lastName, string description, string language)
    {
       var trainer = new Trainer(firstName, lastName, description, language);
       string updatedDescription = description + "!";

       var action = () => trainer.UpdateDescription(updatedDescription);

        action.Should().NotThrow<Exception>();
        trainer.Description.Should().BeEquivalentTo(updatedDescription);
    }

    [Fact]
    public void CantCreateWithInvalidDescription()
    {
        var action = () => new Trainer(string.Concat(Enumerable.Repeat('a', 200)), _fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>().Substring(0,2));

        action.Should().Throw<Exception>();
    }

    [Fact]
    public void CantUpdateWithInvalidDescription()
    {
        string description = "Hello my name is Victor van Duynen";
        var trainer = new Trainer("Victor", "van Duynen",description, "EN" );

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
