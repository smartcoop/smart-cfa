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
        var trainer = _trainerFactory.Create();

        trainer.Enrollments.Should().BeEmpty();
    }

    [Theory]
    [InlineData("Victor", "van Duynen", "Hello my name is Victor van Duynen")]
    public void CanCreateWithValidDescrition(string firstName, string lastName, string description)
    {
        var action = () => new Trainer(firstName, lastName, description);

        action.Should().NotThrow<Exception>();
    }

    [Fact]
    public void CantCreateWithInvalidDescrition()
    {
        var action = () => new Trainer(string.Concat(Enumerable.Repeat('a', 200)), _fixture.Create<string>(), _fixture.Create<string>());

        action.Should().Throw<Exception>();
    }

    [Fact]
    public void CanEnrollInTraining()
    {
        var otherTrainer = _trainerFactory.Create();
        var training = _trainingFactory.Create(otherTrainer);
        var trainerToEnroll = _trainerFactory.Create();

        trainerToEnroll.EnrollIn(training);

        trainerToEnroll.Enrollments.Select(enrollment => enrollment.Training).Should().Contain(training);
    }

    [Fact]
    public void CanDisEnrollFromTraining()
    {
        var training = _trainingFactory.CreateClean();
        var trainerToEnroll = _trainerFactory.Create();
        trainerToEnroll.EnrollIn(training);

        trainerToEnroll.DisenrollFrom(training);

        trainerToEnroll.Enrollments.Should().BeEmpty();
    }

}
