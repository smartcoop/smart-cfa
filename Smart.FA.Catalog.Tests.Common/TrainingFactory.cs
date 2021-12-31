using AutoFixture;
using Core.Domain;

namespace Smart.FA.Catalog.Tests.Common;

public class TrainingFactory
{
    public Training Create(Trainer trainer)
    {
        Fixture fixture = new();
       return new Training(trainer, new List<TrainingType> {TrainingType.Other, TrainingType.Professional},
            TrainingSlotNumberType.Single,
            new List<TrainingTargetAudience> {TrainingTargetAudience.Employee, TrainingTargetAudience.Student});
    }

    public Training CreateClean()
    {
        TrainerFactory trainerFactory = new();
        return Create(trainerFactory.Create());
    }
}
