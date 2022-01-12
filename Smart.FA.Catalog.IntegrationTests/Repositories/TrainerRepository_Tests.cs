using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Core.Domain.Dto;
using FluentAssertions;
using Infrastructure.Persistence.Read;
using Infrastructure.Persistence.Write;
using Microsoft.EntityFrameworkCore;
using Smart.FA.Catalog.IntegrationTests.Base;
using Smart.FA.Catalog.Tests.Common;
using Xunit;

namespace Smart.FA.Catalog.IntegrationTests.Repositories;

[Collection("Integration test collection")]
public class TrainerRepositoryTests : IntegrationTestBase
{
    private readonly TrainerFactory _trainerFactory = new();
    private readonly TrainingFactory _trainingFactory = new();
    private readonly Fixture _fixture = new();
    private readonly TrainerQueries _trainerQueries = new TrainerQueries(ConnectionSetup.Training.ConnectionString);

    public TrainerRepositoryTests()
    {
    }

    [Fact]
    public async Task CanGetListFromTrainingId()
    {
        await using var context = GivenTrainingContext();
        var trainerRepository = new TrainerRepository(context);
        var trainer = _trainerFactory.Create(_fixture.Create<string>(), _fixture.Create<string>());
        context.Trainers.Attach(trainer);
        context.SaveChanges();
        var training = _trainingFactory.Create(trainer);
        context.Trainings.Add(training);
        context.SaveChanges();

        var foundTrainers = await trainerRepository.GetListAsync(training.Id, CancellationToken.None);
        foundTrainers.Should().NotBeEmpty();
        foundTrainers.Should().Contain(trainer);
    }

    [Fact]
    public async Task GetGetFromId()
    {
        await using var context = GivenTrainingContext();
        var trainerRepository = new TrainerRepository(context);
        var trainer = _trainerFactory.Create(_fixture.Create<string>(), _fixture.Create<string>());
        context.Trainers.Attach(trainer);
        await context.SaveChangesAsync();

        var foundTrainer = await trainerRepository.FindAsync(trainer.Id, CancellationToken.None);

        foundTrainer.Should().NotBeNull();
        foundTrainer.Should().BeEquivalentTo(trainer);
    }

    [Fact]
    public async Task GetListOfTrainersFromTrainingId()
    {
        var trainerToAdd = _trainerFactory.Create(_fixture.Create<string>(), _fixture.Create<string>());
        var trainingToAdd = _trainingFactory.Create(trainerToAdd);
        await using (var context = GivenTrainingContext(false))
        {
            context.Trainers.Attach(trainerToAdd);
            await context.SaveChangesAsync();

            context.Trainings.Attach(trainingToAdd);
           await context.SaveChangesAsync();
        }

        var trainers = await _trainerQueries.GetListAsync(new List<int> {trainingToAdd.Id}, CancellationToken.None);

        trainers.Should().NotBeEmpty();
        trainers.Should().Contain(trainer => trainer.Id == trainerToAdd.Id);
    }
}
