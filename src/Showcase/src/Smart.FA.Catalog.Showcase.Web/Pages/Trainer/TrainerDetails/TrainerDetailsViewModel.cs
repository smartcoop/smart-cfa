using Smart.FA.Catalog.Shared.Domain.Enumerations.Trainer;
using Smart.FA.Catalog.Showcase.Web.Pages.Training.TrainingList;

namespace Smart.FA.Catalog.Showcase.Web.Pages.Trainer.TrainerDetails;

public class TrainerDetailsViewModel
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Biography { get; set; } = null!;

    public string? ProfileImagePath { get; set; }
    public List<TrainerSocialNetwork>? SocialNetworks { get; set; } = new ();
    public List<TrainingListViewModel> Trainings { get; set; } = new ();
}

public class TrainerSocialNetwork
{
    public SocialNetwork SocialNetwork { get; set; } = null!;

    public string? SocialNetworkUrl { get; set; }

    public string IconPathFileName { get; set; } = null!;
}
