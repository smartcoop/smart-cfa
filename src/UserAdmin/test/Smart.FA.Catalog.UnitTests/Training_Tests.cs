using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using Smart.FA.Catalog.Core.Domain;
using Smart.FA.Catalog.Core.Domain.Dto;
using Smart.FA.Catalog.Core.Domain.ValueObjects;
using Smart.FA.Catalog.Core.Exceptions;
using Smart.FA.Catalog.Shared.Domain.Enumerations.Training;
using Smart.FA.Catalog.Tests.Common.Factories;
using Xunit;

namespace Smart.FA.Catalog.UnitTests;

public class TrainingTests
{
    private readonly Fixture _fixture = new();

    [Fact]
    public void InstantiatingTraining_ShouldRequireAtLeastOneTrainer()
    {
        var trainer = MockedTrainerFactory.CreateClean();
        var training = MockedTrainingFactory.Create(trainer);

        training.TrainerAssignments.Should().ContainSingle();
        training.TrainerAssignments.Select(assignment => assignment.Trainer).FirstOrDefault().Should()
            .BeSameAs(trainer);
    }

    [Fact]
    public void AssigningValidTrainerToATraining_ShouldAddItToTheTrainingList()
    {
        var trainerAlpha = MockedTrainerFactory.CreateClean();
        var training = MockedTrainingFactory.Create(trainerAlpha);
        var trainer = MockedTrainerFactory.CreateClean();
        trainer.Id = 1;

        training.AssignTrainer(trainer);

        training.TrainerAssignments.Should().HaveCount(2);
    }

    [Fact]
    public void AddingDetailToTraining_WithValidTitle_ShouldNotThrowException_AndAddToDetailsList()
    {
        var trainer = MockedTrainerFactory.CreateClean();
        var training = MockedTrainingFactory.Create(trainer);

        var action = () => training.AddDetails(_fixture.Create<string>(), _fixture.Create<string>(),
            _fixture.Create<string>(), _fixture.Create<string>(),
            Language.Create(_fixture.Create<string>().Substring(0, 2)).Value);

        action.Should().NotThrow<Exception>();
        training.Details.Should().HaveCount(2);
    }

    [Theory]
    [InlineData(null)]
    public void AddingDetailToTraining_WithInvalidTitle_ShouldThrowException(string title)
    {
        var trainer = MockedTrainerFactory.CreateClean();
        var training = MockedTrainingFactory.Create(trainer);

        var action = () => training.AddDetails(title, _fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), Language.Create(_fixture.Create<string>()[..2]).Value);

        action.Should().Throw<Exception>().WithMessage(Errors.Training.EmptyTitle().Message);
    }

    [Theory]
    [InlineData("FR")]
    public void AddingDetailToTraining_WithDuplicateLanguage_ShouldThrowException(string language)
    {
        var trainer = MockedTrainerFactory.CreateClean();
        var duplicateLanguage = Language.Create(language).Value;
        var training = MockedTrainingFactory.Create(trainer, duplicateLanguage);

        var action = () => training.AddDetails(_fixture.Create<string>(), _fixture.Create<string>(),
            _fixture.Create<string>(), _fixture.Create<string>(), duplicateLanguage);

        action.Should().Throw<Exception>().WithMessage("A description for that language already exists");
    }

    [Fact]
    public void UpdatingTrainingLocalizedDetails_ShouldNotThrowException()
    {
        var trainer = MockedTrainerFactory.CreateClean();
        var training = MockedTrainingFactory.Create(trainer);
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
    public void InstantiatingTraining_ShouldHaveADefaultDraftStatus()
    {
        var trainer = MockedTrainerFactory.CreateClean();

        var training = MockedTrainingFactory.Create(trainer);

        training.StatusType.Should().Be(TrainingStatusType.Draft);
    }

    [Fact]
    public void UpdatingTrainingStatus_ToPublished_ShouldSucceed()
    {
        var trainer = MockedTrainerFactory.CreateClean();
        var training = MockedTrainingFactory.CreateWithAutoValidation(trainer);

        training.UpdateDetails("Hello", "My Goal", "A methodology", "practical modalities", Language.Create("FR").Value);
        var result = training.ChangeStatus(TrainingStatusType.Published);

        result.IsSuccess.Should().BeTrue();
        training.StatusType.Should().Be(TrainingStatusType.Published);
    }

    [Fact]
    public void InstantiatingTraining_ShouldAlwaysHaveAtLeastOneVatExemptionClaimType()
    {
        var trainer = MockedTrainerFactory.CreateClean();

        var training = MockedTrainingFactory.Create(trainer);

        training.VatExemptionClaims.Should().NotBeNull();
        training.VatExemptionClaims.Should().NotBeEmpty();
    }

    [Fact]
    public void InstantiatingTraining_ShouldAlwaysComeWithATargetAudience()
    {
        var trainer = MockedTrainerFactory.CreateClean();

        var training = MockedTrainingFactory.Create(trainer);

        training.Targets.Should().NotBeNull();
        training.Targets.Should().NotBeEmpty();
    }

    [Fact]
    public void SwitchingTrainingAttendanceType_FromSingleToGroup_ShouldSucceed()
    {
        var trainer = MockedTrainerFactory.CreateClean();
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
    public void SwitchingTrainingTargetAudience_ToNull_ShouldThrowAnException()
    {
        var training = MockedTrainingFactory.CreateClean();

        var action = () => training.SwitchTargetAudience(null!);

        action.Should().Throw<Exception>().WithMessage(Errors.Training.NoTargetAudience().Message);
    }

    [Fact]
    public void SwitchingTrainingTopics_ToNull_ShouldThrowAnException()
    {
        var training = MockedTrainingFactory.CreateClean();

        var action = () => training.SwitchTopics(null!);

        action.Should().Throw<Exception>().WithMessage(Errors.Training.NoTopics().Message);
    }

    [Fact]
    public void SwitchingTrainingVatExemption_ToNull_ShouldThrowAnException()
    {
        var training = MockedTrainingFactory.CreateClean();

        var action = () => training.SwitchVatExemptionTypes(null!);

        action.Should().Throw<Exception>().WithMessage(Errors.Training.NoVatExemption().Message);
    }

    [Fact]
    public void MarkingATraining_AsGivenBySmart_ShouldSucceed()
    {
        var training = MockedTrainingFactory.CreateClean();

        training.MarkAsGivenBySmart();

        training.IsGivenBySmart.Should().BeTrue();
    }

    [Fact]
    public void AssigningTrainerToTraining_ShouldSucceed()
    {
        var training = MockedTrainingFactory.CreateClean();
        var trainer = MockedTrainerFactory.CreateClean();
        trainer.Id = 1;

        training.AssignTrainer(trainer);

        training.TrainerAssignments.Count.Should().Be(2);
        training.TrainerAssignments.Select(assignment => assignment.Trainer).Should().Contain(trainer);
    }

    [Fact]
    public void AssigningMultipleTrainersToTraining_ShouldSucceed()
    {
        var training = MockedTrainingFactory.CreateClean();
        var trainer1 = MockedTrainerFactory.CreateClean();
        trainer1.Id = 1;
        var trainer2 = MockedTrainerFactory.CreateClean();
        trainer2.Id = 2;
        var trainerList = new List<Trainer> { trainer1, trainer2 };

        training.AssignTrainers(trainerList);

        training.TrainerAssignments.Count.Should().Be(3);
        training.TrainerAssignments.Select(assignment => assignment.Trainer).Should().Contain(trainerList);
    }

    [Fact]
    public void AssigningTwiceTheSameTrainerToTraining_ShouldThrowException()
    {
        var training = MockedTrainingFactory.CreateClean();
        var trainer = MockedTrainerFactory.CreateClean();
        var trainerList = new List<Trainer> { trainer, trainer };

        var action = () => training.AssignTrainers(trainerList);

        action.Should().Throw<Exception>().WithMessage(Errors.Training.AlreadyAssigned(trainer.Name).Message);
    }

    [Fact]
    public void UnAssigningMultipleTrainersToTraining_ShouldMakeItsAssignmentEmpty()
    {
        var training = MockedTrainingFactory.CreateClean();
        var trainer1 = MockedTrainerFactory.CreateClean();
        trainer1.Id = 1;
        var trainer2 = MockedTrainerFactory.CreateClean();
        trainer2.Id = 2;
        var trainerList = new List<Trainer> { trainer1, trainer2 };
        training.AssignTrainers(trainerList);

        training.UnAssignAll();

        training.TrainerAssignments.Should().ContainSingle();
    }

    [Fact]
    public void ChangingTrainingStatus_ToDraftShouldSucceed()
    {
        var training = MockedTrainingFactory.CreateClean();

       training.ChangeStatus(TrainingStatusType.Draft);

       training.StatusType.Should().Be(TrainingStatusType.Draft);
    }

    [Fact]
    public void PublishingTraining_WithValidField_ShouldSucceed()
    {
        var training = MockedTrainingFactory.CreateClean();

        var result = training.ChangeStatus(TrainingStatusType.Published);

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void PublishingTraining_WithValidField_ShouldCreateValidateTrainingDomainEvent()
    {
        var training = MockedTrainingFactory.CreateClean();
        var details = training.Details.FirstOrDefault(training => training.Language == Language.Create("EN").Value) ?? training.Details.First();
        var fakeDomainEvent = new ValidateTrainingEvent(details.Title, training.Id, training.TrainerAssignments.Select(assignment => assignment.TrainerId));

        var result = training.ChangeStatus(TrainingStatusType.Published);

        training.DomainEvents.FirstOrDefault(domainEvent => (domainEvent as ValidateTrainingEvent).TrainingId == fakeDomainEvent.TrainingId).Should().NotBeNull();
    }

    [Fact]
    public void PublishingTraining_WithMissingGoal_ShouldReturnError()
    {
        var trainer = MockedTrainerFactory.CreateClean();
        var training = new Training(trainer,
            new TrainingLocalizedDetailsDto(_fixture.Create<string>(), string.Empty, _fixture.Create<string>()[..2], _fixture.Create<string>(), _fixture.Create<string>()),
            new List<VatExemptionType> { VatExemptionType.Other }, new List<AttendanceType> { AttendanceType.Group }, new List<TargetAudienceType> { TargetAudienceType.Employee },
            new List<Topic> { Topic.Communication });

        var result = training.ChangeStatus(TrainingStatusType.Published);

        result.IsFailure.Should().BeTrue();
        result.Error.Select(error => error.Message).Should().Contain(Errors.General.MissingField("goal").Message);
    }

    [Fact]
    public void PublishingTraining_WithMissingMethodology_ShouldReturnError()
    {
        var trainer = MockedTrainerFactory.CreateClean();
        var training = new Training(trainer,
            new TrainingLocalizedDetailsDto(_fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>()[..2], string.Empty, _fixture.Create<string>()),
            new List<VatExemptionType> { VatExemptionType.Other }, new List<AttendanceType> { AttendanceType.Group }, new List<TargetAudienceType> { TargetAudienceType.Employee },
            new List<Topic> { Topic.Communication });

        var result = training.ChangeStatus(TrainingStatusType.Published);

        result.IsFailure.Should().BeTrue();
        result.Error.Select(error => error.Message).Should().Contain(Errors.General.MissingField("methodology").Message);
    }

    [Fact]
    public void PublishingTraining_WithMissingModalities_ShouldReturnError()
    {
        var trainer = MockedTrainerFactory.CreateClean();
        var training = new Training(trainer,
            new TrainingLocalizedDetailsDto(_fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>()[..2], _fixture.Create<string>(), string.Empty),
            new List<VatExemptionType> { VatExemptionType.Other }, new List<AttendanceType> { AttendanceType.Group }, new List<TargetAudienceType> { TargetAudienceType.Employee },
            new List<Topic> { Topic.Communication });

        var result = training.ChangeStatus(TrainingStatusType.Published);

        result.IsFailure.Should().BeTrue();
        result.Error.Select(error => error.Message).Should().Contain(Errors.General.MissingField("practicalModalities").Message);
    }

    [Fact]
    public void PublishingTraining_WithMissingVatExemptions_ShouldReturnError()
    {
        var trainer = MockedTrainerFactory.CreateClean();
        var training = new Training(trainer,
            new TrainingLocalizedDetailsDto(_fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>()[..2], _fixture.Create<string>(), _fixture.Create<string>()),
            new List<VatExemptionType>(), new List<AttendanceType> { AttendanceType.Group }, new List<TargetAudienceType> { TargetAudienceType.Employee },
            new List<Topic> { Topic.Communication });

        var result = training.ChangeStatus(TrainingStatusType.Published);

        result.IsFailure.Should().BeTrue();
        result.Error.Select(error => error.Message).Should().Contain(Errors.General.MissingField("identity").Message);
    }

    [Fact]
    public void PublishingTraining_WithMissingAttendance_ShouldReturnError()
    {
        var trainer = MockedTrainerFactory.CreateClean();
        var training = new Training(trainer,
            new TrainingLocalizedDetailsDto(_fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>()[..2], _fixture.Create<string>(), _fixture.Create<string>()),
            new List<VatExemptionType> { VatExemptionType.Other }, new List<AttendanceType>(), new List<TargetAudienceType> { TargetAudienceType.Employee },
            new List<Topic> { Topic.Communication });

        var result = training.ChangeStatus(TrainingStatusType.Published);

        result.IsFailure.Should().BeTrue();
        result.Error.Select(error => error.Message).Should().Contain(Errors.General.MissingField("attendance").Message);
    }

    [Fact]
    public void PublishingTraining_WithMissingTargetAudience_ShouldReturnError()
    {
        var trainer = MockedTrainerFactory.CreateClean();
        var training = new Training(trainer,
            new TrainingLocalizedDetailsDto(_fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>()[..2], _fixture.Create<string>(), _fixture.Create<string>()),
            new List<VatExemptionType> { VatExemptionType.Other }, new List<AttendanceType> { AttendanceType.Group }, new List<TargetAudienceType>(),
            new List<Topic> { Topic.Communication });

        var result = training.ChangeStatus(TrainingStatusType.Published);

        result.IsFailure.Should().BeTrue();
        result.Error.Select(error => error.Message).Should().Contain(Errors.General.MissingField("target").Message);
    }

    [Fact]
    public void PublishingTraining_WithMissingTopic_ShouldReturnError()
    {
        var trainer = MockedTrainerFactory.CreateClean();
        var training = new Training(trainer,
            new TrainingLocalizedDetailsDto(_fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>()[..2], _fixture.Create<string>(), _fixture.Create<string>()),
            new List<VatExemptionType> { VatExemptionType.Other }, new List<AttendanceType> { AttendanceType.Group }, new List<TargetAudienceType>(),
            new List<Topic>());

        var result = training.ChangeStatus(TrainingStatusType.Published);

        result.IsFailure.Should().BeTrue();
        result.Error.Select(error => error.Message).Should().Contain(Errors.General.MissingField("topic").Message);
    }
}
