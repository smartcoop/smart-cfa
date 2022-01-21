using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using Core.Domain;
using Core.Domain.Dto;
using Core.Domain.Enumerations;
using Core.Exceptions;
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
        var trainer = _trainerFactory.CreateClean();
        var training = _trainingFactory.Create(trainer);

        training.TrainerEnrollments.Should().ContainSingle();
        training.TrainerEnrollments.Select(enrollment => enrollment.Trainer).FirstOrDefault().Should()
            .BeSameAs(trainer);
    }

    [Fact]
    public void CanAddTrainer()
    {
        var trainerAlpha = _trainerFactory.CreateClean();
        var training = _trainingFactory.Create(trainerAlpha);

        var trainerBeta = _trainerFactory.CreateClean();
        training.EnrollTrainer(trainerBeta);

        training.TrainerEnrollments.Should().HaveCount(2);
    }


    [Fact]
    public void CanAddValidDetails()
    {
        var trainer = _trainerFactory.CreateClean();
        var training = _trainingFactory.Create(trainer);

        var action = () => training.AddDetails(_fixture.Create<string>(), _fixture.Create<string>(),
            _fixture.Create<string>(),
            Language.Create(_fixture.Create<string>().Substring(0, 2)).Value);

        action.Should().NotThrow<Exception>();
        training.Details.Should().HaveCount(2);
    }


    [Theory]
    [InlineData("Title", "Goal", "Methodology", "FRE")]
    public void CantAddInvalidDetails(string title, string goal, string methodology, string language)
    {
        var trainer = _trainerFactory.CreateClean();
        var training = _trainingFactory.Create(trainer);

        var action = () => training.AddDetails(title, goal, methodology,  Language.Create(language).Value);

        action.Should().Throw<Exception>();
        training.Details.Should().HaveCount(1);
    }

    [Fact]
    public void CantAddTwiceSameLanguageDescription()
    {
        var trainer = _trainerFactory.CreateClean();
        var training = _trainingFactory.Create(trainer);
        var language = Language.Create(_fixture.Create<string>().Substring(0, 2)).Value;


        training.AddDetails(_fixture.Create<string>(), _fixture.Create<string>(),
            _fixture.Create<string>(), language);
        var action = () => training.AddDetails(_fixture.Create<string>(), _fixture.Create<string>(),
            _fixture.Create<string>(), language);

        action.Should().Throw<Exception>();
        training.Details.Should().HaveCount(2);
    }


    [Fact]
    public void CanUpdateLanguageDescription()
    {
        var trainer = _trainerFactory.CreateClean();
        var training = _trainingFactory.Create(trainer);
        var language =  Language.Create(_fixture.Create<string>().Substring(0, 2)).Value;
        var newTitle = _fixture.Create<string>();

        training.AddDetails(_fixture.Create<string>(), _fixture.Create<string>(),
            _fixture.Create<string>(), language);
        var action = () => training.UpdateDetails(newTitle, _fixture.Create<string>(),
            _fixture.Create<string>(),language);


        action.Should().NotThrow<Exception>();
        training.Details.Should().HaveCount(2);
        training.Details.FirstOrDefault(detail => detail.Language == language).Should().NotBeNull();
        training.Details.FirstOrDefault(detail => detail.Language == language)!.Title.Should().Be(newTitle);
    }


    [Fact]
    public void StartsInDraft()
    {
        var trainer = _trainerFactory.CreateClean();

        var training = _trainingFactory.Create(trainer);

        training.StatusId.Should().Be(TrainingStatus.Draft.Id);
    }


    [Fact]
    public void StatusCanBeAutoValidated()
    {
        var trainer = _trainerFactory.CreateClean();
        var training = new Training(trainer, new TrainingDetailDto(_fixture.Create<string>(),null,"FR", null),new List<TrainingType> {TrainingType.LanguageCourse},
            new List<TrainingSlotNumberType>{TrainingSlotNumberType.Group}, new List<TrainingTargetAudience> {TrainingTargetAudience.Employee});
        training.UpdateDetails("Hello", "My Goal", "A methodology", Language.Create("FR").Value);

        var errors = training.Validate();

        errors.Should().BeEmpty();
        training.StatusId.Should().Be(TrainingStatus.Validated.Id);
    }

    [Fact]
    public void StatusMustBeManuallyValidated()
    {
        var trainer = _trainerFactory.CreateClean();
        var training = new Training(trainer, new TrainingDetailDto(_fixture.Create<string>(),null,"FR", null),new List<TrainingType> {TrainingType.Professional},
            new List<TrainingSlotNumberType>{TrainingSlotNumberType.Group}, new List<TrainingTargetAudience> {TrainingTargetAudience.Employee});
        training.UpdateDetails("Hello", "My Goal", "A methodology", Language.Create("FR").Value);


        var errors = training.Validate();

        errors.Should().BeEmpty();
        training.StatusId.Should().Be(TrainingStatus.WaitingForValidation.Id);
    }

    [Fact]
    public void HasAlwaysAType()
    {
        var trainer = _trainerFactory.CreateClean();

        var training = _trainingFactory.Create(trainer);

        training.Identities.Should().NotBeNull();
        training.Identities.Should().NotBeEmpty();
    }

    [Fact]
    public void HasAlwaysATargetAudience()
    {
        var trainer = _trainerFactory.CreateClean();

        var training = _trainingFactory.Create(trainer);

        training.Targets.Should().NotBeNull();
        training.Targets.Should().NotBeEmpty();
    }

    [Fact]
    public void CanBeSwitchedFromSingleToGroupSlotNumber()
    {
        var trainer = _trainerFactory.CreateClean();
        var training = new Training(trainer, new TrainingDetailDto(_fixture.Create<string>(),null,"FR", null) , new List<TrainingType> {TrainingType.Professional},
            new List<TrainingSlotNumberType>{TrainingSlotNumberType.Single}, new List<TrainingTargetAudience> {TrainingTargetAudience.Employee});

        training.SwitchSlotNumberType(new List<TrainingSlotNumberType>{TrainingSlotNumberType.Group});

        training.Slots.Should().ContainSingle();
        training.Slots.Select(slot => slot.TrainingSlotNumberSlotType).First().Should().BeSameAs(TrainingSlotNumberType.Group);
    }

    [Fact]
    public void CannotBeSwitchedToNullValue()
    {
        var training = _trainingFactory.CreateClean();

        var action = () => training.SwitchTargetAudience(null!);

        action.Should().Throw<GuardClauseException>();
    }
}
