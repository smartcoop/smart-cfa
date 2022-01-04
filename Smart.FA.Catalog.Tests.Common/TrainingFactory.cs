using AutoFixture;
using Core.Domain;

namespace Smart.FA.Catalog.Tests.Common;

public class TrainingFactory
{
    public Training Create(Trainer trainer)
    {
        Fixture fixture = new();

       var training = new Training(trainer, new List<TrainingType> {TrainingType.SchoolCourse, TrainingType.Professional},
            TrainingSlotNumberType.Single,
            new List<TrainingTargetAudience> {TrainingTargetAudience.Employee, TrainingTargetAudience.Student});
       return training;
    }

    public Training CreateClean()
    {
        TrainerFactory trainerFactory = new();
        return Create(trainerFactory.CreateClean());
    }


}
