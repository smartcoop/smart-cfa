using Smart.FA.Catalog.Shared.Domain.Enumerations.Training;

namespace Smart.FA.Catalog.Showcase.Web.Pages.Training.TrainingList;

public class TrainingListViewModel
{
    public int TrainingId { get; set; }
    public string Title { get; set; }
    public int TrainerId { get; set; }
    public string TrainerFirstName { get; set; }
    public string TrainerLastName { get; set; }
    public List<Topic> Topics { get; set; } = new List<Topic>();
    public List<string> Languages { get; set; } = new List<string>();
    public TrainingStatusType Status { get; set; }
    public bool IsGivenBySmart { get; set; }
}
