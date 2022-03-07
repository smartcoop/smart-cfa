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
    public async Task CanCreate(string firstName, string lastName)
    {
        await using var context = GivenCatalogContext(false);
        var trainer = _trainerFactory.Create(firstName, lastName);
        context.Trainers.Attach(trainer);
        var training = _trainingFactory.Create(trainer);
        context.Trainings.Attach(training);
        await context.SaveChangesAsync();

        var completeTraining = await context.Trainings.FindAsync(training.Id);

        completeTraining.Should().NotBeNull();
        completeTraining!.TrainerAssignments.Should().NotBeNull();
        completeTraining.TrainerAssignments.Select(tt => tt.Trainer).Should().Contain(trainer);
    }
}
