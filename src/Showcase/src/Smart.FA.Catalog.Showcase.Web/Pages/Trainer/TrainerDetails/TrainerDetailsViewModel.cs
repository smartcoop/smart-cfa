using Smart.FA.Catalog.Shared.Domain.Enumerations.Trainer;
using Smart.FA.Catalog.Showcase.Web.Pages.Training.TrainingList;

namespace Smart.FA.Catalog.Showcase.Web.Pages.Trainer.TrainerDetails;

public class TrainerDetailsViewModel
{
    public int TrainerId { get; set; }
    public string TrainerFirstName { get; set; }
    public string TrainerLastName { get; set; }
    public string TrainerTitle { get; set; }
    public string TrainerBiography { get; set; }
    public List<TrainerSocialNetwork> TrainerSocialNetworks { get; set; } = new ();
    public List<TrainingListViewModel> TrainerTrainings { get; set; } = new ();

}

public class TrainerSocialNetwork
{
    public SocialNetwork SocialNetwork { get; set; }
    public string SocialNetworkUrl { get; set; }
}
