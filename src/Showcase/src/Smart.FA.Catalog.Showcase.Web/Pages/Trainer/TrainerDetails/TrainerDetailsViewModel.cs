using Smart.FA.Catalog.Shared.Collections;
using Smart.FA.Catalog.Shared.Domain.Enumerations.Trainer;
using Smart.FA.Catalog.Showcase.Web.Pages.Training.TrainingList;

namespace Smart.FA.Catalog.Showcase.Web.Pages.Trainer.TrainerDetails;

public class TrainerDetailsViewModel
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Title { get; set; }
    public string Biography { get; set; }
    public string ProfileImagePath { get; set; }
    public List<TrainerSocialNetwork> SocialNetworks { get; set; } = new ();
    public PagedList<TrainingListViewModel> Trainings { get; set; }
}

public class TrainerSocialNetwork
{
    public SocialNetwork SocialNetwork { get; set; }
    public string SocialNetworkUrl { get; set; }
    public string IconPathFileName { get; set; }
    
}
