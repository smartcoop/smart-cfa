using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using Smart.FA.Catalog.Application.UseCases.Commands;
using Smart.FA.Catalog.Core.Domain.User.Dto;
using Smart.FA.Catalog.Core.Domain.User.Enumerations;
using Smart.FA.Catalog.Core.Domain.ValueObjects;
using Smart.FA.Catalog.Infrastructure.Helpers;
using Smart.FA.Catalog.Infrastructure.Services.Options;
using Smart.FA.Catalog.IntegrationTests.Base;
using Smart.FA.Catalog.Tests.Common.Extensions;
using Smart.FA.Catalog.Tests.Common.Factories;
using Xunit;

namespace Smart.FA.Catalog.IntegrationTests.TrainingContext;

[Collection(IntegrationTestCollections.Default)]
public class TrainerTests : IntegrationTestBase
{
    private readonly Fixture _fixture = new();

    [Theory]
    [JsonFileData("data.json", "ValidTrainerProfile")]
    public async Task CanCreateTrainer(string userId, string firstName, string lastName, string applicationType, string email)
    {
        await using var context = GivenCatalogContext();
        var user = new UserDto(userId, _fixture.Create<string>(), _fixture.Create<string>(), applicationType, $"{Guid.NewGuid()}@gmail.com");
        var trainer = MockedTrainerFactory.CreateFromUser(user);
        context.Trainers.Attach(trainer);
        await context.SaveChangesAsync();
        var foundTrainer = await context.Trainers.FindAsync(trainer.Id);

        foundTrainer.Should().NotBeNull();
        foundTrainer!.Should().BeSameAs(trainer);
        foundTrainer!.Name.FirstName.Should().NotBeEmpty();
        foundTrainer.Name.LastName.Should().NotBeEmpty();
        foundTrainer.DefaultLanguage.Value.Should().NotBeEmpty();
    }

    [Theory]
    [InlineData("Victor", "vD")]
    public async Task CanChangeTrainerName(string firstName, string lastName)
    {
        await using var context = GivenCatalogContext();
        var trainer = MockedTrainerFactory.Create(firstName, lastName);
        context.Trainers.Attach(trainer);
        await context.SaveChangesAsync();
        var newName = Name.Create("Maxime", "P.");

        trainer.Rename(newName.Value);
        var foundTrainer = await context.Trainers.FindAsync(trainer.Id);

        foundTrainer!.Name.Should().BeSameAs(newName.Value);
    }

    [Fact]
    public async Task CanCreateFromUser()
    {
        await using var context = GivenCatalogContext();
        var user = MockedUserFactory.CreateClean();
        var trainer = MockedTrainerFactory.CreateFromUser(user);

        context.Trainers.Add(trainer);
        await context.SaveChangesAsync();
        var trackedTrainer = await context.Trainers.FindAsync(trainer.Id);

        trackedTrainer.Should().NotBeNull();
        trackedTrainer!.Name.Should().BeEquivalentTo(trainer.Name);
        trackedTrainer.DefaultLanguage.Should().BeEquivalentTo(trainer.DefaultLanguage);
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData("1", null)]
    [InlineData(null, 1)]
    public async Task CantCreateFromInvalidUser(string? userId, int? applicationTypeId)
    {
        var action = async () =>
        {
            await using var context = GivenCatalogContext();

            var applicationName = applicationTypeId is null ? null : ApplicationType.FromValue((int)applicationTypeId).Name;
            var user = new UserDto(userId!, _fixture.Create<string>(), _fixture.Create<string>(), applicationName!, $"{Guid.NewGuid()}@gmail.com");
            var trainer = MockedTrainerFactory.CreateFromUser(user);
            context.Trainers.Add(trainer);
            await context.SaveChangesAsync();
        };

        await action.Should().ThrowAsync<Exception>();
    }

    [Theory]
    [JsonFileData("data.json", "ValidTrainerProfile")]
    public async Task CanCreateTrainerFromUserApp(string userId, string firstName, string lastName, string applicationType, string email)
    {
        await using var context = GivenCatalogContext(false);
        var handler = new CreateTrainerFromUserAppCommandHandler(MockedLoggerFactory.Create<CreateTrainerFromUserAppCommandHandler>(), context);
        var request = new CreateTrainerFromUserAppRequest { User = new UserDto(userId, firstName, lastName, applicationType, email) };

        var response = await handler.Handle(request, CancellationToken.None);

        response.IsSuccess.Should().BeTrue();
        response.Trainer.Should().NotBeNull();
        response.Trainer.Identity.Should().Be(TrainerIdentity.Create(userId, ApplicationType.FromName(applicationType)).Value);
        response.Trainer.Name.Should().Be(Name.Create(firstName, lastName).Value);
        response.Trainer.Email.Should().Be(email);
    }

    [Theory]
    [JsonFileData("data.json", "ValidTrainerProfileDescription")]
    public async Task EditProfileOfTrainer(string bio, string title)
    {
        await using var context = GivenCatalogContext(false);
        var trainerToUpdate = MockedTrainerFactory.CreateClean();
        context.Trainers.Add(trainerToUpdate);
        await context.SaveChangesAsync();
        var handler = new EditProfileCommandHandler(Substitute.For<ILogger<EditProfileCommandHandler>>(), context, new MinIoLinkGenerator(Substitute.For<IOptions<S3StorageOptions>>()));
        var socialNetworks = JsonFileTestExtensions.CreateDataSet<Dictionary<string, string>>("data.json", "ValidSocialNetworks").FirstOrDefault() ?? new Dictionary<string, string>();
        var request = new EditProfileCommand { Bio = bio, Socials = socialNetworks, Title = title, TrainerId = trainerToUpdate.Id };

        var response = await handler.Handle(request, CancellationToken.None);
        var trainerWithEditedProfile = await context.Trainers.FirstOrDefaultAsync(trainer => trainerToUpdate.Id == trainer.Id);

        response.IsSuccess.Should().BeTrue();
        response.Found.Should().BeTrue();
        trainerWithEditedProfile.Should().NotBeNull();
        trainerWithEditedProfile!.Biography.Should().Be(bio);
        trainerWithEditedProfile.Title.Should().Be(title);
        trainerWithEditedProfile.SocialNetworks.Select(social => social.SocialNetwork.Id).Should().Contain(socialNetworks.Select(social =>
        {
            if (!int.TryParse(social.Key, out var socialKey))
            {
                throw new Exception();
            }

            return socialKey;
        }));
    }
}
