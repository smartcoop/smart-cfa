using Smart.FA.Catalog.Shared.Domain.Enumerations.Training;

namespace Smart.FA.Catalog.Core.Domain.Enumerations;

public static class TrainingTypeExtensions
{
    public static bool IsTrainingAutoValidated(this IEnumerable<TrainingIdentity> trainingTypes)
        => trainingTypes.Select(identity => identity.TrainingType).Contains(TrainingType.LanguageCourse);
}
