using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Smart.FA.Catalog.IntegrationTests.Base;
using Smart.FA.Catalog.Tests.Common;
using Xunit;

namespace Smart.FA.Catalog.IntegrationTests.TrainingContext;

[Collection("Integration test collection")]
public class TrainingTests: IntegrationTestBase
{
    private readonly TrainerFactory _trainerFactory = new();
    private readonly TrainingFactory _trainingFactory = new();

    [Theory]
    [InlineData("Victor", "vD")]
    public async Task CanCreateTraining(string firstName, string lastName)
    {
        using var context = GivenTrainingContext(false);
        var trainer = _trainerFactory.Create(firstName, lastName);
        context.Trainers.Attach(trainer);
        await context.SaveChangesAsync();

        var training = _trainingFactory.Create(trainer);
        context.Trainings.Attach(training);
        await context.SaveChangesAsync();

        var completeTraining = await context.Trainings.FindAsync(training.Id);

        completeTraining.Should().NotBeNull();
        completeTraining!.TrainerEnrollments.Should().NotBeNull();
        completeTraining.TrainerEnrollments.Select(tt => tt.Trainer).Should().Contain(trainer);
    }
}
