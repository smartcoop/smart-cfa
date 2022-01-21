using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Infrastructure.Persistence.Read;
using Infrastructure.Persistence.Write;
using Microsoft.Data.SqlClient;
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
    private readonly TrainerQueries _trainerQueries = new(ConnectionSetup.Training.ConnectionString);

    [Fact]
    public async Task GetListFromTrainingId()
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
    public async Task GetReadOnlyListFromTrainingId()
    {
        await using var context = GivenTrainingContext(false);
        var trainerToAdd = _trainerFactory.Create(_fixture.Create<string>(), _fixture.Create<string>());
        context.Trainers.Attach(trainerToAdd);
        var trainingToAdd = _trainingFactory.Create(trainerToAdd);
        context.Trainings.Attach(trainingToAdd);
        await context.SaveChangesAsync();

        var trainers = (await _trainerQueries
                .GetListAsync(new List<int> {trainingToAdd.Id}, CancellationToken.None))
            .ToList();

        trainers.Should().NotBeEmpty();
        trainers.Should().Contain(trainer => trainer.Id == trainerToAdd.Id);
    }

    [Fact]
    public async Task GetFromId()
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
}
