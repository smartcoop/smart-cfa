using System;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using Smart.FA.Catalog.Core.Domain;
using Smart.FA.Catalog.Core.Domain.User.Enumerations;
using Smart.FA.Catalog.Core.Domain.ValueObjects;
using Smart.FA.Catalog.Core.Exceptions;
using Smart.FA.Catalog.Shared.Domain.Enumerations.Trainer;
using Smart.FA.Catalog.Tests.Common.Factories;
using Xunit;

namespace Smart.FA.Catalog.UnitTests;

public class TrainerTests
{
    private readonly Fixture _fixture = new();

    [Fact]
    public void InstantiatingATrainer_ShouldStartWithEmptyAssignment()
    {
        var trainer = MockedTrainerFactory.CreateClean();

        trainer.Assignments.Should().BeEmpty();
    }

    [Theory]
    [InlineData("A super biography")]
    public void UpdatingTrainer_WithValidBiography_ShouldNotThrowExceptionAndUpdateTrainer(string biography)
    {
        var trainer = MockedTrainerFactory.CreateClean();

        var action = () => trainer.UpdateBiography(biography);

        action.Should().NotThrow<Exception>();
        trainer.Biography.Should().BeEquivalentTo(biography);
    }

    [Fact]
    public void UpdatingTrainer_WithNullBiography_ShouldThrowException()
    {
        var trainer = MockedTrainerFactory.CreateClean();

        var action = () => trainer.UpdateBiography(null!);

        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void UpdatingTrainer_WithNullTitle_ShouldThrowException()
    {
        var trainer = MockedTrainerFactory.CreateClean();

        var action = () => trainer.UpdateTitle(null!);

        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void UpdatingTrainer_WithNullProfileUrl_ShouldThrowException()
    {
        var trainer = MockedTrainerFactory.CreateClean();

        var action = () => trainer.UpdateProfileImagePath(null!);

        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void UpdatingTrainer_WithBiographyTooBig_ShouldThrowException()
    {
        var trainer = MockedTrainerFactory.CreateClean();
        var newBiography = string.Concat(Enumerable.Repeat('a', 501));

        var action = () => trainer.UpdateBiography(newBiography);

        action.Should().Throw<Exception>().WithMessage(Errors.Trainer.BiographyIsTooLong(trainer.Id, newBiography.Length, 500).Message);
    }

    [Fact]
    public void UpdatingTrainer_WithTitleTooBig_ShouldThrowException()
    {
        var trainer = MockedTrainerFactory.CreateClean();
        var newTitle = string.Concat(Enumerable.Repeat('a', 151));

        var action = () => trainer.UpdateTitle(newTitle);

        action.Should().Throw<Exception>(Errors.Trainer.TitleIsTooLong(trainer.Id, newTitle.Length, 150).Message);
    }

    [Fact]
    public void UpdatingTrainer_WithProfileUrlTooBig_ShouldThrowException()
    {
        var trainer = MockedTrainerFactory.CreateClean();
        var newUrl = string.Concat(Enumerable.Repeat('a', 51));

        var action = () => trainer.UpdateProfileImagePath(newUrl);

        action.Should().Throw<Exception>(Errors.Trainer.ProfileImage.UrlTooLong(trainer.Id, newUrl.Length, 50).Message);
    }

    [Fact]
    public void UpdatingTrainer_WithValidProfileUrl_ShouldSucceed()
    {
        var trainer = MockedTrainerFactory.CreateClean();
        var newUrl = _fixture.Create<string>();

        var action = () => trainer.UpdateProfileImagePath(newUrl);

        action.Should().NotThrow<Exception>();
        trainer.ProfileImagePath.Should().Be(newUrl);
    }

    [Fact]
    public void UpdatingTrainer_WithValidTitle_ShouldSucceed()
    {
        var trainer = MockedTrainerFactory.CreateClean();
        var newTitle = _fixture.Create<string>();

        var action = () => trainer.UpdateTitle(newTitle);

        action.Should().NotThrow<Exception>();
        trainer.Title.Should().Be(newTitle);
    }


    [Fact]
    public void RenamingTrainer_ShouldSucceed()
    {
        var trainer = MockedTrainerFactory.CreateClean();
        var newName = Name.Create("test_firstName", "test_lastName").Value;

        trainer.Rename(newName);

        trainer.Name.Should().Be(newName);
    }

    [Fact]
    public void ChangingDefaultLanguageOfTrainer_ShouldSucceed()
    {
        var trainer = MockedTrainerFactory.CreateClean();
        var newLanguage = Language.Create("PT").Value;

        trainer.ChangeDefaultLanguage(newLanguage);

        trainer.DefaultLanguage.Should().Be(newLanguage);
    }

    [Fact]
    public void UpdatingTrainer_WithInvalidBiography_ShouldThrowException()
    {
        var description = _fixture.Create<string>();
        var trainer = new Trainer(Name.Create(_fixture.Create<string>(), _fixture.Create<string>()).Value,
            TrainerIdentity.Create(_fixture.Create<string>(), ApplicationType.Account).Value, _fixture.Create<string>(), description, Language.Create(_fixture.Create<string>()[..2]).Value);

        var action = () => trainer.UpdateBiography(string.Concat(Enumerable.Repeat('a', 2000)));

        action.Should().Throw<Exception>();
    }

    [Fact]
    public void AssigningTrainer_ToTheSameTrainingTwice_ShouldThrowException()
    {
        var trainer = MockedTrainerFactory.CreateClean();
        var training = MockedTrainingFactory.Create(trainer);

        trainer.AssignTo(training);
        var action = () => trainer.AssignTo(training);

        action.Should().Throw<Exception>().WithMessage(Errors.Trainer.TrainerAlreadyAssignedToTraining(trainer.Id, training.Id).Message);
    }

    [Fact]
    public void AssigningTrainer_ShouldAssignItToTraining()
    {
        var otherTrainer = MockedTrainerFactory.CreateClean();
        var training = MockedTrainingFactory.Create(otherTrainer);
        var trainerToAssign = MockedTrainerFactory.CreateClean();

        trainerToAssign.AssignTo(training);

        trainerToAssign.Assignments.Select(assignment => assignment.Training).Should().Contain(training);
    }

    [Fact]
    public void UnAssigningTrainerFromTraining_ShouldSucceed()
    {
        var training = MockedTrainingFactory.CreateClean();
        var trainerToUnAssign = MockedTrainerFactory.CreateClean();
        trainerToUnAssign.AssignTo(training);

        trainerToUnAssign.UnAssignFrom(training);

        trainerToUnAssign.Assignments.Should().BeEmpty();
    }

    [Fact]
    public void ApprovingNullUserChart_ShouldThrowException()
    {
        var trainerToUnAssign = MockedTrainerFactory.CreateClean();

        var action = () => trainerToUnAssign.ApproveUserChart(null);

        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ApprovingSameUserChartForTrainer_ShouldNotThrowException()
    {
        var trainerToUnAssign = MockedTrainerFactory.CreateClean();
        var userChartRevision = MockedUserChartRevisionFactory.Create();

        trainerToUnAssign.ApproveUserChart(userChartRevision);
        var action = () => trainerToUnAssign.ApproveUserChart(userChartRevision);

        action.Should().NotThrow<Exception>();
    }

    [Fact]
    public void ChangingTrainerWithEmail_ShouldSucceed()
    {
        var trainer = MockedTrainerFactory.CreateClean();
        var email = "test@gmail.com";

        var action = () => trainer.ChangeEmail(email);

        action.Should().NotThrow<Exception>();
        trainer.Email.Should().Be(email);
    }

    [Fact]
    public void ChangingTrainerWithNullEmail_ShouldThrowException()
    {
        var trainer = MockedTrainerFactory.CreateClean();

        var action = () => trainer.ChangeEmail(null);

        action.Should().Throw<ArgumentNullException>();
    }

    [Theory]
    [InlineData("&é&é&@gmail.com")]
    public void ChangingTrainerWithInvalidEmail_ShouldThrowException(string email)
    {
        var trainer = MockedTrainerFactory.CreateClean();

        var action = () => trainer.ChangeEmail(email);

        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void SettingNewSocialNetworkToTrainer_ShouldSucceed()
    {
        var trainer = MockedTrainerFactory.CreateClean();
        trainer.Id = 1;
        var socialNetwork = SocialNetwork.Twitter;
        string url = _fixture.Create<string>();

        var action = () => trainer.SetSocialNetwork(socialNetwork, url);

        action.Should().NotThrow<Exception>();
        trainer.SocialNetworks.Select(socialNetwork => socialNetwork.SocialNetwork).Should().Contain(socialNetwork);
    }

    [Fact]
    public void SettingExistingSocialNetworkToTrainer_ShouldSucceedAndUpdateUrl()
    {
        var trainer = MockedTrainerFactory.CreateClean();
        trainer.Id = 1;
        var socialNetwork = SocialNetwork.Twitter;
        string url = _fixture.Create<string>();
        string newUrl = _fixture.Create<string>();

        trainer.SetSocialNetwork(socialNetwork, url);
        var action = () => trainer.SetSocialNetwork(socialNetwork, newUrl);

        action.Should().NotThrow<Exception>();
        trainer.SocialNetworks.Select(socialNetwork => socialNetwork.SocialNetwork).Should().Contain(socialNetwork);
        trainer.SocialNetworks.Select(socialNetwork => socialNetwork.UrlToProfile).Should().Contain(newUrl);
    }

    [Fact]
    public void SettingExistingSocialNetwork_WithNullUrlToProfileToTrainer_ShouldSucceedAndRemoveSocialNetwork()
    {
        var trainer = MockedTrainerFactory.CreateClean();
        trainer.Id = 1;
        var socialNetwork = SocialNetwork.Twitter;
        string url = _fixture.Create<string>();

        trainer.SetSocialNetwork(socialNetwork, url);
        var action = () => trainer.SetSocialNetwork(socialNetwork, null);

        action.Should().NotThrow<Exception>();
        trainer.SocialNetworks.Select(socialNetwork => socialNetwork.SocialNetwork).Should().NotContain(socialNetwork);
    }

    [Fact]
    public void SettingNewSocialNetworkToTransientTrainer_ShouldThrowException()
    {
        var trainer = MockedTrainerFactory.CreateClean();
        var socialNetwork = SocialNetwork.Twitter;
        string url = _fixture.Create<string>();

        var action = () => trainer.SetSocialNetwork(socialNetwork, url);

        action.Should().Throw<Exception>().WithMessage(Errors.Trainer.TrainerIsTransient().Message);
    }

    [Fact]
    public void SettingNullSocialNetworkToTrainer_ShouldThrowException()
    {
        var trainer = MockedTrainerFactory.CreateClean();
        trainer.Id = 1;
        string url = _fixture.Create<string>();

        var action = () => trainer.SetSocialNetwork(null!, url);

        action.Should().Throw<ArgumentNullException>();
    }
}
