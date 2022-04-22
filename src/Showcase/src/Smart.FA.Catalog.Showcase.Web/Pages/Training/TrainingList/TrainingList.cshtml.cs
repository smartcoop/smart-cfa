#nullable disable
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Smart.FA.Catalog.Shared.Domain.Enumerations.Training;

namespace Smart.FA.Catalog.Showcase.Web.Pages.Training;

public class TrainingListModel : PageModel
{
    private readonly Infrastructure.Data.CatalogShowcaseContext _context;

    public TrainingListModel(Infrastructure.Data.CatalogShowcaseContext context)
    {
        _context = context;
    }

    public List<TrainingListViewModel> Trainings { get; set; } = new List<TrainingListViewModel>();

    public async Task OnGetAsync()
    {
        var trainingList = await _context.TrainingList
            .Where(training => training.TrainingStatus == TrainingStatusType.Validated.Id)
            .OrderBy(t => t.TrainingId)
            .ToListAsync();

        var trainingsByIds = trainingList.ToLookup(t => t.TrainingId);

        foreach (var groupedTraining in trainingsByIds)
        {
            Trainings.Add(new TrainingListViewModel()
            {
                TrainingId = groupedTraining.Key,
                TrainingTitle = groupedTraining.FirstOrDefault().TrainingTitle,
                TrainerFirstName = groupedTraining.FirstOrDefault().TrainerFirstName,
                TrainerLastName = groupedTraining.FirstOrDefault().TrainerLastName,
                TrainingStatusType = TrainingStatusType.FromValue<TrainingStatusType>(groupedTraining.FirstOrDefault().TrainingStatus),
                Topics = groupedTraining.Select(x => Topic.FromValue<Topic>(x.TrainingTopic))
                    .ToList(),
                TrainingLanguages = groupedTraining.Select(x => (x.TrainingLanguage)).Distinct().ToList()
            });
        }
    }
}
