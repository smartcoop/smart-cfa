using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Smart.FA.Catalog.Infrastructure.Persistence.Read;
using Smart.FA.Catalog.Infrastructure.Persistence.Write;
using Smart.FA.Catalog.IntegrationTests.Base;
using Smart.FA.Catalog.Tests.Common;
using Xunit;

namespace Smart.FA.Catalog.IntegrationTests.Repositories;

[Collection(IntegrationTestCollections.Default)]
public class TrainerRepositoryTests : IntegrationTestBase
{
    private readonly TrainerQueries _trainerQueries = new(Connection.Catalog.ConnectionString);

    private readonly Fixture _fixture = new();
    [Fact]
    public async Task GetListFromTrainingId()
    {
        await using var context = GivenCatalogContext();
        var trainerRepository = new TrainerRepository(context);
        var trainer = TrainerFactory.Create(_fixture.Create<string>(), _fixture.Create<string>());
        context.Trainers.Attach(trainer);
        await context.SaveChangesAsync();
        var training = TrainingFactory.Create(trainer);
        context.Trainings.Attach(training);
        await context.SaveChangesAsync();

        var foundTrainers = await trainerRepository.GetListAsync(training.Id, CancellationToken.None);

        foundTrainers.Should().NotBeEmpty();
        foundTrainers.Should().Contain(trainer);
    }

    [Fact]
    public async Task GetReadOnlyListFromTrainingId()
    {
        await using var context = GivenCatalogContext(false);
        var trainerToAdd = TrainerFactory.Create(_fixture.Create<string>(), _fixture.Create<string>());
        context.Trainers.Attach(trainerToAdd);
        var trainingToAdd = TrainingFactory.Create(trainerToAdd);
        context.Trainings.Attach(trainingToAdd);
        await context.SaveChangesAsync();

        var trainers = await _trainerQueries.GetListAsync(new List<int> { trainingToAdd.Id, 2 }, CancellationToken.None);

        trainers.Should().NotBeEmpty();
        trainers.Should().Contain(trainer => trainer.Id == trainerToAdd.Id);
    }

    [Fact]
    public async Task GetFromId()
    {
        await using var context = GivenCatalogContext();
        var trainerRepository = new TrainerRepository(context);
        var trainer = TrainerFactory.Create(_fixture.Create<string>(), _fixture.Create<string>());
        context.Trainers.Attach(trainer);
        await context.SaveChangesAsync();

        var foundTrainer = await trainerRepository.FindAsync(trainer.Id, CancellationToken.None);

        foundTrainer.Should().NotBeNull();
        foundTrainer.Should().BeEquivalentTo(trainer);
    }
}
