using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Smart.FA.Catalog.Application.UseCases.Queries;
using Smart.FA.Catalog.Core.Domain.ValueObjects;
using Smart.FA.Catalog.IntegrationTests.Base;
using Smart.FA.Catalog.Shared.Collections;
using Smart.FA.Catalog.Tests.Common.Factories;
using Xunit;

namespace Smart.FA.Catalog.IntegrationTests.Repositories;

[Collection(IntegrationTestCollections.Default)]
public class TrainingRepositoryTests : IntegrationTestBase
{
    private readonly Fixture _fixture = new();

    [Fact]
    public async Task GetPaginatedReadOnlyListFromTrainerId()
    {
        var pagedItem = new PageItem(1, 1);
        var context = GivenCatalogContext();
        var trainerToAdd = MockedTrainerFactory.CreateClean();
        context.Trainers.Attach(trainerToAdd);
        var language = Language.Create(_fixture.Create<string>().Substring(0, 2)).Value;
        for (int trainerCount = 0; trainerCount < 10; trainerCount++)
        {
            var trainingToAdd = MockedTrainingFactory.Create(trainerToAdd, language);
            context.Trainings.Attach(trainingToAdd);
        }
        await context.SaveChangesAsync();
        var handler = new GetPagedTrainingListFromTrainerQueryHandler(MockedLoggerFactory.Create<GetPagedTrainingListFromTrainerQueryHandler>(), context);

        var pagedTrainingList = await handler
            .Handle(new GetPagedTrainingListFromTrainerRequest { Language = language, PageItem = pagedItem, TrainerId = trainerToAdd.Id }, CancellationToken.None);

        pagedTrainingList.Trainings.Should().NotBeEmpty();
        pagedTrainingList.Trainings.Should().HaveCountLessOrEqualTo(pagedItem.PageSize);
        pagedTrainingList.Trainings.Should().HaveCountLessOrEqualTo(pagedTrainingList.Trainings.TotalCount);
    }
}
