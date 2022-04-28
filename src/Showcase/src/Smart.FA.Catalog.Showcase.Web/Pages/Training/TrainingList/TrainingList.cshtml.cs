#nullable disable
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Smart.FA.Catalog.Shared.Domain.Enumerations.Training;

namespace Smart.FA.Catalog.Showcase.Web.Pages.Training.TrainingList;

public class TrainingListModel : PageModel
{
    private readonly Infrastructure.Data.CatalogShowcaseContext _context;
    public List<TrainingListViewModel> Trainings { get; set; } = new List<TrainingListViewModel>();


    public TrainingListModel(Infrastructure.Data.CatalogShowcaseContext context)
    {
        _context = context;
    }

    public async Task OnGetAsync()
    {
        var trainingList = await _context.TrainingList
            .Where(training => training.Status == TrainingStatus.Validated.Id)
            .OrderBy(t => t.TrainingId)
            .ToListAsync();

        var trainingsByIds = trainingList.ToLookup(t => t.TrainingId);

        foreach (var groupedTraining in trainingsByIds)
        {
            Trainings.Add(new TrainingListViewModel()
            {
                TrainingId = groupedTraining.Key,
                Title = groupedTraining.FirstOrDefault().Title,
                TrainerFirstName = groupedTraining.FirstOrDefault().TrainerFirstName,
                TrainerLastName = groupedTraining.FirstOrDefault().TrainerLastName,
                Status = TrainingStatus.FromValue<TrainingStatus>(groupedTraining.FirstOrDefault().Status),
                Topics = groupedTraining.Select(x => TrainingTopic.FromValue<TrainingTopic>(x.Topic)).ToList(),
                Languages = groupedTraining.Select(x => (x.Language)).Distinct().ToList()
            });
        }
    }
}
