using System.Linq;
using System.Threading.Tasks;
using Core.Domain;
using FluentAssertions;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Smart.FA.Catalog.IntegrationTests.Base;
using Smart.FA.Catalog.Tests.Common;
using Xunit;

namespace Smart.FA.Catalog.IntegrationTests;

[Collection("Integration test collection")]
public class TrainerTests : IntegrationTestBase
{
    private readonly TrainerFactory _trainerFactory = new();
    private readonly TrainingFactory _trainingFactory = new();

    public TrainerTests()
    {
    }

    [Theory]
    [InlineData("Victor", "vD")]
    public async Task CanGetTrainer(string firstName, string lastName)
    {
        using var context = GivenTrainingContext();
        var trainer = _trainerFactory.Create(firstName, lastName);

        context.Trainers.Add(trainer);
        await context.SaveChangesAsync();
        Trainer? foundTrainer = await context.Trainers.FirstOrDefaultAsync(trainer => trainer.FirstName == firstName);

        foundTrainer.Should().NotBeNull();
    }

    [Theory]
    [InlineData("Victor", "vD")]
    public async Task CanGetTraining(string firstName, string lastName)
    {
        using var context = GivenTrainingContext(false);
        var trainer = _trainerFactory.Create(firstName, lastName);
        context.Trainers.Add(trainer);
        await context.SaveChangesAsync();

        var training = _trainingFactory.Create(trainer);
        context.Trainings.Add(training);
        await context.SaveChangesAsync();

        var completeTraining = await context.Trainings.Include(training => training.TrainerEnrollments)
           .FirstOrDefaultAsync();

        completeTraining.Should().NotBeNull();
        completeTraining!.TrainerEnrollments.Should().NotBeNull();
        completeTraining.TrainerEnrollments.Select(tt => tt.Trainer).Should().Contain(trainer);

    }
}
