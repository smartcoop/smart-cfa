using Smart.FA.Catalog.Shared.Domain.Enumerations.Training;

namespace Smart.FA.Catalog.Showcase.Web.Pages.Training.TrainingDetails;

public class TrainingDetailsViewModel
{
    public int Id { get; set; }
    public string TrainingTitle { get; set; }
    public string Methodology { get; set; }
    public string Goal { get; set; }
    public string Language { get; set; }
    public string PracticalModalities { get; set; }
    public string TrainerFirstName { get; set; }
    public string TrainerLastName { get; set; }
    public string TrainerTitle { get; set; }
    public List<TrainingTopic> Topics { get; set; } = new List<TrainingTopic>();
    public List<string> Languages { get; set; } = new List<string>();
    public TrainingStatus Status { get; set; }
}
