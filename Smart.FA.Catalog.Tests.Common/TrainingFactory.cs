using AutoFixture;
using Core.Domain;
using Core.Domain.Dto;
using Core.Domain.Enumerations;

namespace Smart.FA.Catalog.Tests.Common;

public class TrainingFactory
{
    public Training Create(Trainer trainer)
    {
        Fixture fixture = new();
        return new Training
        (
            trainer
            , new TrainingDetailDto(fixture.Create<string>(), null, "FR", null)
            , new List<TrainingType> {TrainingType.Professional}
            , new List<TrainingSlotNumberType> {TrainingSlotNumberType.Group}
            , new List<TrainingTargetAudience> {TrainingTargetAudience.Employee}
            , new List<TrainingTopic> {TrainingTopic.Communication}
        );
    }

    public Training CreateClean()
    {
        TrainerFactory trainerFactory = new();
        return Create(trainerFactory.CreateClean());
    }

    public Training CreateWithManualValidation(Trainer trainer)
    {
        Fixture fixture = new();
        return new Training
        (
            trainer
            , new TrainingDetailDto(fixture.Create<string>(), null, "FR", null)
            , new List<TrainingType> {TrainingType.Professional}
            , new List<TrainingSlotNumberType> {TrainingSlotNumberType.Group}
            , new List<TrainingTargetAudience> {TrainingTargetAudience.Employee}
            , new List<TrainingTopic> {TrainingTopic.Communication}
        );
    }

    public Training CreateWithAutoValidation(Trainer trainer)
    {
        Fixture fixture = new();
        return new Training
        (
            trainer
            , new TrainingDetailDto(fixture.Create<string>(), null, "FR", null)
            , new List<TrainingType> {TrainingType.LanguageCourse}
            , new List<TrainingSlotNumberType> {TrainingSlotNumberType.Group}
            , new List<TrainingTargetAudience> {TrainingTargetAudience.Employee}
            , new List<TrainingTopic> {TrainingTopic.Communication}
        );
    }
}
