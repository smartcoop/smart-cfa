using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Core.Domain;
using Core.Domain.Dto;
using Core.Domain.Enumerations;
using Core.SeedWork;
using FluentAssertions;
using Infrastructure.Persistence.Read;
using Microsoft.Data.SqlClient;
using Smart.FA.Catalog.IntegrationTests.Base;
using Smart.FA.Catalog.Tests.Common;
using Xunit;

namespace Smart.FA.Catalog.IntegrationTests.Repositories;

[Collection("Integration test collection")]
public class TrainingRepositoryTests : IntegrationTestBase
{
    private readonly TrainerFactory _trainerFactory = new();
    private readonly TrainingFactory _trainingFactory = new();
    private readonly Fixture _fixture = new();
    private readonly TrainingQueries _trainingQueries = new(ConnectionSetup.Training.ConnectionString);

    [Fact]
    public async Task GetPaginatedReadOnlyListFromTrainerId()
    {
        int pageSize = 1;
        int currentPage = 1;
        await using var context = GivenTrainingContext(false);
        var trainerToAdd = _trainerFactory.Create(_fixture.Create<string>(), _fixture.Create<string>());
        context.Trainers.Attach(trainerToAdd);
        var language = Language.Create(_fixture.Create<string>().Substring(0, 2)).Value;
        var trainingToAdd = new Training(trainerToAdd, new TrainingDetailDto(_fixture.Create<string>(),null,language.Value, null),new List<TrainingType> {TrainingType.LanguageCourse},
            new List<TrainingSlotNumberType>{TrainingSlotNumberType.Group}, new List<TrainingTargetAudience> {TrainingTargetAudience.Employee});
        context.Trainings.Attach(trainingToAdd);
        await context.SaveChangesAsync();

        var pagedTrainingList = await _trainingQueries
            .GetPagedListAsync(trainerToAdd.Id, language.Value, new PageItem(currentPage, pageSize),
                CancellationToken.None);

        pagedTrainingList.Should().NotBeEmpty();
        pagedTrainingList.Should().HaveCountLessOrEqualTo(pageSize);
        pagedTrainingList.Should().HaveCountLessOrEqualTo(pagedTrainingList.TotalCount);
    }
}
