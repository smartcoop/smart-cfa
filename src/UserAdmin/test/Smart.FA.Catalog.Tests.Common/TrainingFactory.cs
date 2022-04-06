using AutoFixture;
using Smart.FA.Catalog.Core.Domain;
using Smart.FA.Catalog.Core.Domain.Dto;
using Smart.FA.Catalog.Core.Domain.Enumerations;
using Smart.FA.Catalog.Shared.Domain.Enumerations.Training;

namespace Smart.FA.Catalog.Tests.Common;

public static class TrainingFactory
{
    private static Fixture fixture = new();

    public  static Training Create(Trainer trainer)
    {
        return new Training
        (
            trainer
            , new TrainingDetailDto(fixture.Create<string>(), null, "FR", null, null)
            , new List<TrainingType> {TrainingType.Professional}
            , new List<TrainingSlotNumberType> {TrainingSlotNumberType.Group}
            , new List<TrainingTargetAudience> {TrainingTargetAudience.Employee}
            , new List<TrainingTopic> {TrainingTopic.Communication}
        );
    }

    public static Training CreateClean()
    {
        return Create(TrainerFactory.CreateClean());
    }

    public static Training CreateWithManualValidation(Trainer trainer)
    {
        return new Training
        (
            trainer
            , new TrainingDetailDto(fixture.Create<string>(), null, "FR", null, null)
            , new List<TrainingType> {TrainingType.Professional}
            , new List<TrainingSlotNumberType> {TrainingSlotNumberType.Group}
            , new List<TrainingTargetAudience> {TrainingTargetAudience.Employee}
            , new List<TrainingTopic> {TrainingTopic.Communication}
        );
    }

    public static Training CreateWithAutoValidation(Trainer trainer)
    {
        return new Training
        (
            trainer
            , new TrainingDetailDto(fixture.Create<string>(), null, "FR", null, null)
            , new List<TrainingType> {TrainingType.LanguageCourse}
            , new List<TrainingSlotNumberType> {TrainingSlotNumberType.Group}
            , new List<TrainingTargetAudience> {TrainingTargetAudience.Employee}
            , new List<TrainingTopic> {TrainingTopic.Communication}
        );
    }
}
