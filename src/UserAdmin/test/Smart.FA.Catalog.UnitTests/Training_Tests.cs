using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using NSubstitute;
using Smart.FA.Catalog.Core.Domain;
using Smart.FA.Catalog.Core.Domain.Dto;
using Smart.FA.Catalog.Core.Domain.Enumerations;
using Smart.FA.Catalog.Core.Domain.ValueObjects;
using Smart.FA.Catalog.Core.Exceptions;
using Smart.FA.Catalog.Core.Services;
using Smart.FA.Catalog.Shared.Domain.Enumerations.Training;
using Smart.FA.Catalog.Tests.Common;
using Xunit;

namespace Smart.FA.Catalog.UnitTests;

public class TrainingTests
{
    private Fixture _fixture = new();
    private readonly IMailService _mailService;

    public TrainingTests()
    {
        _mailService = Substitute.For<IMailService>();
    }

    [Fact]
    public void HasInitiallyOneTrainer()
    {
        var trainer = TrainerFactory.CreateClean();
        var training = TrainingFactory.Create(trainer);

        training.TrainerAssignments.Should().ContainSingle();
        training.TrainerAssignments.Select(assignment => assignment.Trainer).FirstOrDefault().Should()
            .BeSameAs(trainer);
    }

    [Fact]
    public void CanAddTrainer()
    {
        var trainerAlpha = TrainerFactory.CreateClean();
        var training = TrainingFactory.Create(trainerAlpha);

        var trainerBeta = TrainerFactory.CreateClean();
        training.AssignTrainer(trainerBeta);

        training.TrainerAssignments.Should().HaveCount(2);
    }


    [Fact]
    public void CanAddValidDetails()
    {
        var trainer = TrainerFactory.CreateClean();
        var training = TrainingFactory.Create(trainer);

        var action = () => training.AddDetails(_fixture.Create<string>(), _fixture.Create<string>(),
            _fixture.Create<string>(),_fixture.Create<string>(),
            Language.Create(_fixture.Create<string>().Substring(0, 2)).Value);

        action.Should().NotThrow<Exception>();
        training.Details.Should().HaveCount(2);
    }


    [Theory]
    [InlineData("Title", "Goal", "Methodology", "PracticalModalities", "FRE")]
    public void CantAddInvalidDetails(string title, string goal, string methodology, string practicalModalities, string language)
    {
        var trainer = TrainerFactory.CreateClean();
        var training = TrainingFactory.Create(trainer);

        var action = () => training.AddDetails(title, goal, methodology, practicalModalities, Language.Create(language).Value);

        action.Should().Throw<Exception>();
        training.Details.Should().HaveCount(1);
    }

    [Fact]
    public void CantAddTwiceSameLanguageDescription()
    {
        var trainer = TrainerFactory.CreateClean();
        var training = TrainingFactory.Create(trainer);
        var language = Language.Create(_fixture.Create<string>().Substring(0, 2)).Value;


        training.AddDetails(_fixture.Create<string>(), _fixture.Create<string>(),
            _fixture.Create<string>(), _fixture.Create<string>(), language);
        var action = () => training.AddDetails(_fixture.Create<string>(), _fixture.Create<string>(),
            _fixture.Create<string>(), _fixture.Create<string>(), language);

        action.Should().Throw<Exception>();
        training.Details.Should().HaveCount(2);
    }


    [Fact]
    public void CanUpdateLanguageDescription()
    {
        var trainer = TrainerFactory.CreateClean();
        var training = TrainingFactory.Create(trainer);
        var language = Language.Create(_fixture.Create<string>().Substring(0, 2)).Value;
        var newTitle = _fixture.Create<string>();

        training.AddDetails(_fixture.Create<string>(), _fixture.Create<string>(),
            _fixture.Create<string>(), _fixture.Create<string>(), language);
        var action = () => training.UpdateDetails(newTitle, _fixture.Create<string>(),
            _fixture.Create<string>(),_fixture.Create<string>(), language);


        action.Should().NotThrow<Exception>();
        training.Details.Should().HaveCount(2);
        training.Details.FirstOrDefault(details => details.Language == language).Should().NotBeNull();
        training.Details.FirstOrDefault(details => details.Language == language)!.Title.Should().Be(newTitle);
    }


    [Fact]
    public void StartsInDraft()
    {
        var trainer = TrainerFactory.CreateClean();

        var training = TrainingFactory.Create(trainer);

        training.StatusType.Should().Be(TrainingStatusType.Draft);
    }


    [Fact]
    public void StatusCanBeAutoValidated()
    {
        var trainer = TrainerFactory.CreateClean();
        var training = TrainingFactory.CreateWithAutoValidation(trainer);

        training.UpdateDetails("Hello", "My Goal", "A methodology", "practical modalities",Language.Create("FR").Value);
        var result = training.ChangeStatus();

        result.IsSuccess.Should().BeTrue();
        training.StatusType.Should().Be(TrainingStatusType.Validated);
    }

    [Fact]
    public void StatusMustBeManuallyValidated()
    {
        var trainer = TrainerFactory.CreateClean();
        var training = TrainingFactory.CreateWithManualValidation(trainer);
        training.UpdateDetails("Hello", "My Goal", "A methodology", "practical modalities", Language.Create("FR").Value);

        var result = training.ChangeStatus();

        result.IsSuccess.Should().BeTrue();
        training.StatusType.Should().Be(TrainingStatusType.WaitingForValidation);
    }

    [Fact]
    public void HasAlwaysAType()
    {
        var trainer = TrainerFactory.CreateClean();

        var training = TrainingFactory.Create(trainer);

        training.VatExemptionClaims.Should().NotBeNull();
        training.VatExemptionClaims.Should().NotBeEmpty();
    }

    [Fact]
    public void HasAlwaysATargetAudience()
    {
        var trainer = TrainerFactory.CreateClean();

        var training = TrainingFactory.Create(trainer);

        training.Targets.Should().NotBeNull();
        training.Targets.Should().NotBeEmpty();
    }

    [Fact]
    public void AttendanceTypeCanBeSwitchedFromSingleToGroup()
    {
        var trainer = TrainerFactory.CreateClean();
        var training = new Training
        (
            trainer
            , new TrainingLocalizedDetailsDto(_fixture.Create<string>(), null, "FR", null, null)
            , new List<VatExemptionType> {VatExemptionType.Professional}
            , new List<AttendanceType> {AttendanceType.Single}
            , new List<TargetAudienceType> {TargetAudienceType.Employee}
            , new List<Topic> {Topic.Communication}
        );

        training.SwitchAttendanceTypes(new List<AttendanceType> {AttendanceType.Group});

        training.Attendances.Should().ContainSingle();
        training.Attendances.Select(trainingAttendance => trainingAttendance.AttendanceType).First().Should()
            .BeSameAs(AttendanceType.Group);
    }

    [Fact]
    public void CannotBeSwitchedToNullValue()
    {
        var training = TrainingFactory.CreateClean();

        var action = () => training.SwitchTargetAudience(null!);

        action.Should().Throw<GuardClauseException>();
    }
}
