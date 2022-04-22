using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Smart.FA.Catalog.Core.Domain;
using Smart.FA.Catalog.Core.Domain.Dto;
using Smart.FA.Catalog.Core.Domain.Enumerations;
using Smart.FA.Catalog.Core.Domain.ValueObjects;
using Smart.FA.Catalog.Core.SeedWork;
using Smart.FA.Catalog.Infrastructure.Persistence.Read;
using Smart.FA.Catalog.IntegrationTests.Base;
using Smart.FA.Catalog.Shared.Domain.Enumerations.Training;
using Smart.FA.Catalog.Tests.Common;
using Xunit;

namespace Smart.FA.Catalog.IntegrationTests.Repositories;

[Collection("Integration test collection")]
public class TrainingRepositoryTests : IntegrationTestBase
{
    private readonly Fixture _fixture = new();
    private readonly TrainingQueries _trainingQueries = new(ConnectionSetup.Training.ConnectionString);

    [Fact]
    public async Task GetPaginatedReadOnlyListFromTrainerId()
    {
        var pageSize = 1;
        var currentPage = 1;
        await using var context = GivenCatalogContext(false);
        var trainerToAdd = TrainerFactory.Create(_fixture.Create<string>(), _fixture.Create<string>());
        context.Trainers.Attach(trainerToAdd);
        var language = Language.Create(_fixture.Create<string>().Substring(0, 2)).Value;
        var trainingToAdd = new Training
        (
            trainerToAdd
            , new TrainingDetailDto
            (
                _fixture.Create<string>(),
                null,
                language.Value,
                null,
                null
            )
            , new List<VatExemptionType> {VatExemptionType.LanguageCourse}
            , new List<AttendanceType> {AttendanceType.Group}
            , new List<TrainingTargetAudience> {TrainingTargetAudience.Employee}
            , new List<Topic>() {Topic.Communication}
        );
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
