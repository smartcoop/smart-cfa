using Smart.FA.Catalog.Shared.Domain.Enumerations.Training;
using Smart.FA.Catalog.Showcase.Web.Pages.Training.TrainingList;

namespace Smart.FA.Catalog.Showcase.Web.Mappers;

public static class TrainingMapper
{
    public static List<TrainingListViewModel> ToTrainingListViewModels(this IEnumerable<Domain.Models.TrainingList> trainingList)
    {
        if (trainingList is null)
        {
            return null;
        }

        var trainings = new List<TrainingListViewModel>(trainingList.Count());
        var trainingsByIds = trainingList.ToLookup(t => t.Id);

        //We only want to map the trainings we'll display in the page and get rid of the others.
        var trainingsRanged = trainingsByIds;

        foreach (var groupedTraining in trainingsRanged)
        {
            // Since we grouped we are sure we have one record at least.
            var firstLine = groupedTraining.First();
            trainings.Add(new TrainingListViewModel
            {
                TrainingId = groupedTraining.Key,
                Title = firstLine.Title,
                TrainerFirstName = firstLine.TrainerFirstName,
                TrainerLastName = firstLine.TrainerLastName,
                Status = TrainingStatusType.FromValue(firstLine.Status),
                Topics = groupedTraining.Select(trainerList => Topic.FromValue(trainerList.Topic)).ToList(),
                Languages = groupedTraining.Select(trainerList => trainerList.Language).Distinct().ToList()
            });
        }

        return trainings;
    }

}
