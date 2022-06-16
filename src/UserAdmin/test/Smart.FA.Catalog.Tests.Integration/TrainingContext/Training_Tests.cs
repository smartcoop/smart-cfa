using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Smart.FA.Catalog.Tests.Common.Factories;
using Smart.FA.Catalog.Tests.Integration.Base;
using Xunit;

namespace Smart.FA.Catalog.Tests.Integration.TrainingContext;

[Collection(IntegrationTestCollections.Default)]
public class TrainingTests : IntegrationTestBase
{
    [Theory]
    [InlineData("Victor", "vD")]
    public async Task CanCreate(string firstName, string lastName)
    {
        await using var context = GivenCatalogContext(false);
        var trainer = MockedTrainerFactory.Create(firstName, lastName);
        context.Trainers.Attach(trainer);
        var training = MockedTrainingFactory.Create(trainer);
        context.Trainings.Attach(training);
        await context.SaveChangesAsync();

        var completeTraining = await context.Trainings.FindAsync(training.Id);

        completeTraining.Should().NotBeNull();
        completeTraining!.TrainerAssignments.Should().NotBeNull();
        completeTraining.TrainerAssignments.Select(tt => tt.Trainer).Should().Contain(trainer);
    }
}
