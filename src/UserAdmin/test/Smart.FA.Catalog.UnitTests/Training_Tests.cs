using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using Smart.FA.Catalog.Core.Domain;
using Smart.FA.Catalog.Core.Domain.Dto;
using Smart.FA.Catalog.Core.Domain.ValueObjects;
using Smart.FA.Catalog.Shared.Domain.Enumerations.Training;
using Smart.FA.Catalog.Tests.Common;
using Smart.FA.Catalog.UnitTests.Data;
using Xunit;

namespace Smart.FA.Catalog.UnitTests;

public class TrainingTests
{
    private readonly Fixture _fixture = new();

    [Fact]
    public void TrainingHasInitiallyOneTrainer()
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
    public void CanAddValidDetailsToTraining()
    {
        var trainer = TrainerFactory.CreateClean();
        var training = TrainingFactory.Create(trainer);

        var action = () => training.AddDetails(_fixture.Create<string>(), _fixture.Create<string>(),
            _fixture.Create<string>(), _fixture.Create<string>(),
            Language.Create(_fixture.Create<string>().Substring(0, 2)).Value);

        action.Should().NotThrow<Exception>();
        training.Details.Should().HaveCount(2);
    }

    [Theory]
    [JsonFileData("data.json", "Training")]
    public void CantAddTrainingWithInvalidDetails(string title, string goal, string methodology, string practicalModalities, string language)
    {
        var trainer = TrainerFactory.CreateClean();
        var training = TrainingFactory.Create(trainer);

        var action = () => training.AddDetails(title, goal, methodology, practicalModalities, Language.Create(language).Value);

        action.Should().Throw<Exception>();
        training.Details.Should().HaveCount(1);
    }

    [Fact]
    public void CantAddTrainingWithTwiceTheSameLanguageDescription()
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
    public void CanUpdateTrainingLanguageDescription()
    {
        var trainer = TrainerFactory.CreateClean();
        var training = TrainingFactory.Create(trainer);
        var language = Language.Create(_fixture.Create<string>().Substring(0, 2)).Value;
        var newTitle = _fixture.Create<string>();

        training.AddDetails(_fixture.Create<string>(), _fixture.Create<string>(),
            _fixture.Create<string>(), _fixture.Create<string>(), language);
        var action = () => training.UpdateDetails(newTitle, _fixture.Create<string>(),
            _fixture.Create<string>(), _fixture.Create<string>(), language);


        action.Should().NotThrow<Exception>();
        training.Details.Should().HaveCount(2);
        training.Details.FirstOrDefault(details => details.Language == language).Should().NotBeNull();
        training.Details.FirstOrDefault(details => details.Language == language)!.Title.Should().Be(newTitle);
    }

    [Fact]
    public void TrainingStartsInDraft()
    {
        var trainer = TrainerFactory.CreateClean();

        var training = TrainingFactory.Create(trainer);

        training.StatusType.Should().Be(TrainingStatusType.Draft);
    }

    [Fact]
    public void TrainingStatusCanBeAutoValidated()
    {
        var trainer = TrainerFactory.CreateClean();
        var training = TrainingFactory.CreateWithAutoValidation(trainer);

        training.UpdateDetails("Hello", "My Goal", "A methodology", "practical modalities", Language.Create("FR").Value);
        var result = training.ChangeStatus(TrainingStatusType.Published);

        result.IsSuccess.Should().BeTrue();
        training.StatusType.Should().Be(TrainingStatusType.Published);
    }

    [Fact]
    public void TrainingHasAlwaysAType()
    {
        var trainer = TrainerFactory.CreateClean();

        var training = TrainingFactory.Create(trainer);

        training.VatExemptionClaims.Should().NotBeNull();
        training.VatExemptionClaims.Should().NotBeEmpty();
    }

    [Fact]
    public void TrainingHasAlwaysATargetAudience()
    {
        var trainer = TrainerFactory.CreateClean();

        var training = TrainingFactory.Create(trainer);

        training.Targets.Should().NotBeNull();
        training.Targets.Should().NotBeEmpty();
    }

    [Fact]
    public void TrainingAttendanceTypeCanBeSwitchedFromSingleToGroup()
    {
        var trainer = TrainerFactory.CreateClean();
        var training = new Training
        (
            trainer
            , new TrainingLocalizedDetailsDto(_fixture.Create<string>(), null, "FR", null, null)
            , new List<VatExemptionType> { VatExemptionType.Professional }
            , new List<AttendanceType> { AttendanceType.Single }
            , new List<TargetAudienceType> { TargetAudienceType.Employee }
            , new List<Topic> { Topic.Communication }
        );

        training.SwitchAttendanceTypes(new List<AttendanceType> { AttendanceType.Group });

        training.Attendances.Should().ContainSingle();
        training.Attendances.Select(trainingAttendance => trainingAttendance.AttendanceType).First().Should()
            .BeSameAs(AttendanceType.Group);
    }

    [Fact]
    public void TrainingCannotBeSwitchedToNullValue()
    {
        var training = TrainingFactory.CreateClean();

        var action = () => training.SwitchTargetAudience(null!);

        action.Should().Throw<Exception>();
    }
}
