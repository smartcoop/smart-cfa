using Smart.FA.Catalog.Shared.Domain.Enumerations.Trainer;
using Smart.FA.Catalog.Showcase.Domain.Models;
using Smart.FA.Catalog.Showcase.Web.Options;
using Smart.FA.Catalog.Showcase.Web.Pages.Trainer.TrainerDetails;
using Smart.FA.Catalog.Showcase.Web.Pages.Trainer.TrainerList;

namespace Smart.FA.Catalog.Showcase.Web.Mappers;

public static class TrainerMapper
{
    public static List<TrainerSocialNetwork> ToTrainerSocialNetworks(this List<TrainerDetails> trainerDetails)
    {
        var socialNetworks = new List<TrainerSocialNetwork>();
        foreach (var detail in trainerDetails.Where(details => !string.IsNullOrWhiteSpace(details.UrlToProfile))
                     .OrderByDescending(s => s.SocialNetwork))
        {
            socialNetworks.Add(detail.ToTrainerSocialNetwork());
        }

        return socialNetworks;
    }

    public static TrainerSocialNetwork ToTrainerSocialNetwork(this TrainerDetails details)
    {
        var socialNetworkName = SocialNetwork.FromValue(details.SocialNetwork!.Value);

        return new TrainerSocialNetwork()
        {
            SocialNetwork = socialNetworkName,
            SocialNetworkUrl = details.UrlToProfile,
            IconPathFileName = $"/icons/{socialNetworkName}.svg"
        };
    }

    public static List<TrainerListViewModel> ToTrainerListViewModels(this ICollection<TrainerList> trainerList, MinIOOptions minIoOptions)
    {
        return trainerList.Select(trainer => new TrainerListViewModel
        {
            Id = trainer.Id,
            FirstName = trainer.FirstName,
            LastName = trainer.LastName,
            Title = trainer.Title,
            ProfileImagePath = minIoOptions.GenerateMinIoTrainerProfileUrl(trainer.ProfileImagePath),
        }).ToList();
    }
}
