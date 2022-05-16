using Smart.FA.Catalog.Shared.Domain.Enumerations.Trainer;
using Smart.FA.Catalog.Showcase.Domain.Models;
using Smart.FA.Catalog.Showcase.Web.Pages.Trainer.TrainerDetails;
using Smart.FA.Catalog.Showcase.Web.Pages.Trainer.TrainerList;

namespace Smart.FA.Catalog.Showcase.Web.Mappers;

public static class TrainerMapper
{
    public static List<TrainerSocialNetwork> ToTrainerSocialNetworks(this List<Domain.Models.TrainerDetails> trainerDetails)
    {
        var socialNetworks = new List<TrainerSocialNetwork>();
        foreach (var detail in trainerDetails.Where(details => !string.IsNullOrWhiteSpace(details.UrlToProfile))
                     .OrderByDescending(s => s.SocialNetwork))
        {
            var socialNetworkName = SocialNetwork.FromValue((int)detail.SocialNetwork);
            socialNetworks.Add(new TrainerSocialNetwork()
            {
                SocialNetwork = socialNetworkName,
                SocialNetworkUrl = detail.UrlToProfile,
                IconPathFileName = $"/icons/{socialNetworkName.ToString().ToLower()}.svg"
            });
        }
        
        return socialNetworks;
    }

    public static List<TrainerListViewModel> ToTrainerListViewModels(this ICollection<TrainerList> trainerList)
    {
        return trainerList.Select(trainer => new TrainerListViewModel
        {
            Id = trainer.Id,
            FirstName = trainer.FirstName,
            LastName = trainer.LastName,
            Title = trainer.Title,
            ProfileImagePath = trainer.ProfileImagePath,
            }).ToList();
    }
}
