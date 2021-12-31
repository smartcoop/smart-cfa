using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using Core.Domain;
using Core.Services;
using FluentAssertions;
using Moq;
using Smart.FA.Catalog.Tests.Common;
using Xunit;

namespace Smart.FA.Catalog.UnitTests;

public class TrainingTests
{
    private TrainingFactory _trainingFactory = new();
    private TrainerFactory _trainerFactory = new();
    private Fixture _fixture = new();
    private readonly Mock<IMailService> _mailService;

    public TrainingTests()
    {
        _mailService = new Mock<IMailService>();
    }

    [Fact]
    public void HasInitiallyOneTrainer()
    {
        var trainer = _trainerFactory.Create();
        var training = _trainingFactory.Create(trainer);

        training.TrainerEnrollments.Should().ContainSingle();
        training.TrainerEnrollments.Select(enrollment => enrollment.Trainer).FirstOrDefault().Should()
            .BeSameAs(trainer);
    }

    [Fact]
    public void CanAddTrainer()
    {
        var trainerAlpha = _trainerFactory.Create();
        var training = _trainingFactory.Create(trainerAlpha);

        var trainerBeta = _trainerFactory.Create();
        training.EnrollTrainer(trainerBeta);

        training.TrainerEnrollments.Should().HaveCount(2);
    }


    [Fact]
    public void CanAddValidDetails()
    {
        var trainer = _trainerFactory.Create();
        var training = _trainingFactory.Create(trainer);

        var action = () => training.AddDetails(_fixture.Create<string>(), _fixture.Create<string>(),
            _fixture.Create<string>(),
            _fixture.Create<string>().Substring(0, 2));

        action.Should().NotThrow<Exception>();
        training.Details.Should().ContainSingle();
    }


    [Theory]
    [InlineData("Title", "Goal", "Methodology", "FRE")]
    public void CantAddInvalidDetails(string title, string goal, string methodology, string language)
    {
        var trainer = _trainerFactory.Create();
        var training = _trainingFactory.Create(trainer);

        var action = () => training.AddDetails(title, goal, methodology, language);

        action.Should().Throw<Exception>();
        training.Details.Should().BeEmpty();
    }

    [Fact]
    public void CantAddTwiceSameLanguageDescription()
    {
        var trainer = _trainerFactory.Create();
        var training = _trainingFactory.Create(trainer);
        var language = _fixture.Create<string>().Substring(0, 2);


        training.AddDetails(_fixture.Create<string>(), _fixture.Create<string>(),
            _fixture.Create<string>(),language);
        var action = () => training.AddDetails(_fixture.Create<string>(), _fixture.Create<string>(),
            _fixture.Create<string>(), language);

        action.Should().Throw<Exception>();
        training.Details.Should().ContainSingle();
    }


    [Fact]
    public void CanUpdateLanguageDescription()
    {
        var trainer = _trainerFactory.Create();
        var training = _trainingFactory.Create(trainer);
        var language = _fixture.Create<string>().Substring(0, 2);
        var newTitle = _fixture.Create<string>();

        training.AddDetails(_fixture.Create<string>(), _fixture.Create<string>(),
            _fixture.Create<string>(),language);
        var action = () => training.UpdateDetails(newTitle, _fixture.Create<string>(),
            _fixture.Create<string>(),language);


        action.Should().NotThrow<Exception>();
        training.Details.Should().ContainSingle();
        training.Details.FirstOrDefault().Should().NotBeNull();
        training.Details.FirstOrDefault()!.Title.Should().Be(newTitle);
    }


    [Fact]
    public void StartsInDraft()
    {
        var trainer = _trainerFactory.Create();

        var training = _trainingFactory.Create(trainer);

        training.Status.Should().Be(TrainingStatus.Draft);
    }


    [Fact]
    public void StatusCanBeAutoValidated()
    {
        var trainer = _trainerFactory.Create();
        var training = new Training(trainer, new List<TrainingType> {TrainingType.LanguageCourse},
            TrainingSlotNumberType.Group, new List<TrainingTargetAudience> {TrainingTargetAudience.Employee});

        training.Validate(_mailService.Object);

        training.Status.Should().Be(TrainingStatus.Validated);
    }

    [Fact]
    public void StatusMustBeManuallyValidated()
    {
        var trainer = _trainerFactory.Create();
        var training = new Training(trainer, new List<TrainingType> {TrainingType.Other},
            TrainingSlotNumberType.Group, new List<TrainingTargetAudience> {TrainingTargetAudience.Employee});

        training.Validate(_mailService.Object);

        training.Status.Should().Be(TrainingStatus.WaitingForValidation);
    }

    [Fact]
    public void HasAlwaysAType()
    {
        var trainer = _trainerFactory.Create();

        var training = _trainingFactory.Create(trainer);

        training.Identities.Should().NotBeNull();
        training.Identities.Should().NotBeEmpty();
    }

    [Fact]
    public void HasAlwaysATargetAudience()
    {
        var trainer = _trainerFactory.Create();

        var training = _trainingFactory.Create(trainer);

        training.Targets.Should().NotBeNull();
        training.Targets.Should().NotBeEmpty();
    }

    [Fact]
    public void CanBeSwitchedFromSingleToGroupSlotNumber()
    {
        var trainer = _trainerFactory.Create();
        var training = new Training(trainer, new List<TrainingType> {TrainingType.Other},
            TrainingSlotNumberType.Single, new List<TrainingTargetAudience> {TrainingTargetAudience.Employee});

        training.SwitchSlotNumberType(TrainingSlotNumberType.Group);

        training.SlotNumberType.Should().Be(TrainingSlotNumberType.Group);
    }
}
