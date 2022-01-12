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

       var training = new Training(trainer,   new TrainingDetailDto(fixture.Create<string>(),null,"FR", null),new List<TrainingType> {TrainingType.SchoolCourse, TrainingType.Professional},
            new List<TrainingSlotNumberType>{TrainingSlotNumberType.Single},
            new List<TrainingTargetAudience> {TrainingTargetAudience.Employee, TrainingTargetAudience.Student});
       return training;
    }

    public Training CreateClean()
    {
        TrainerFactory trainerFactory = new();
        return Create(trainerFactory.CreateClean());
    }
}
