using AutoFixture;
using Smart.FA.Catalog.Core.Domain;
using Smart.FA.Catalog.Core.Domain.Dto;
using Smart.FA.Catalog.Core.Domain.ValueObjects;
using Smart.FA.Catalog.Shared.Domain.Enumerations.Training;

namespace Smart.FA.Catalog.Tests.Common.Factories;

public static class MockedTrainingFactory
{
    private static Fixture fixture = new();

    public  static Training Create(Trainer trainer, Language? language = null)
    {
        return new Training
        (
            trainer
            , new TrainingLocalizedDetailsDto(fixture.Create<string>(), fixture.Create<string>(), language?.Value ?? Language.Create("FR").Value.Value, fixture.Create<string>(), fixture.Create<string>())
            , new List<VatExemptionType> {VatExemptionType.Professional}
            , new List<AttendanceType> {AttendanceType.Group}
            , new List<TargetAudienceType> {TargetAudienceType.Employee}
            , new List<Topic> {Topic.Communication}
        );
    }

    public static Training CreateClean()
    {
        return Create(MockedTrainerFactory.CreateClean());
    }

    public static Training CreateWithManualValidation(Trainer trainer)
    {
        return new Training
        (
            trainer
            , new TrainingLocalizedDetailsDto(fixture.Create<string>(), fixture.Create<string>(), "FR", fixture.Create<string>(), fixture.Create<string>())
            , new List<VatExemptionType> {VatExemptionType.Professional}
            , new List<AttendanceType> {AttendanceType.Group}
            , new List<TargetAudienceType> {TargetAudienceType.Employee}
            , new List<Topic> {Topic.Communication}
        );
    }

    public static Training CreateWithAutoValidation(Trainer trainer)
    {
        return new Training
        (
            trainer
            , new TrainingLocalizedDetailsDto(fixture.Create<string>(), fixture.Create<string>(), "FR", fixture.Create<string>(), fixture.Create<string>())
            , new List<VatExemptionType> {VatExemptionType.LanguageCourse}
            , new List<AttendanceType> {AttendanceType.Group}
            , new List<TargetAudienceType> {TargetAudienceType.Employee}
            , new List<Topic> {Topic.Communication}
        );
    }
}
