using Smart.FA.Catalog.Shared.Domain.Enumerations.Training;

namespace Smart.FA.Catalog.Showcase.Web.Pages.Training.TrainingDetails;

public class TrainingDetailsViewModel
{
    public int TrainingId { get; set; }
    public string TrainingTitle { get; set; }
    public string TrainingMethodology { get; set; }
    public string TrainingGoal { get; set; }
    public string TrainingLanguage { get; set; }
    public string TrainingPracticalModalities { get; set; }
    public string TrainerFirstName { get; set; }
    public string TrainerLastName { get; set; }
    public string TrainerTitle { get; set; }
    public List<TrainingTopic> Topics { get; set; } = new List<TrainingTopic>();
    public List<string> TrainingLanguages { get; set; } = new List<string>();
    public TrainingStatus TrainingStatus { get; set; }
}
