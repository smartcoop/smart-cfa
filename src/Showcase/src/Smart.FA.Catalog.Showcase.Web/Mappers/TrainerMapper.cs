using Smart.FA.Catalog.Shared.Domain.Enumerations.Trainer;
using Smart.FA.Catalog.Showcase.Domain.Models;
using Smart.FA.Catalog.Showcase.Web.Pages.Trainer.TrainerDetails;

namespace Smart.FA.Catalog.Showcase.Web.Mappers;

public static class TrainerMapper
{
    public static TrainerDetailsViewModel ToTrainerDetailsViewModel(this IEnumerable<TrainerDetails> trainerDetails,
        List<TrainingList> trainerTrainingList)
    {
        var firstLine = trainerDetails.FirstOrDefault();

        if (firstLine is null)
        {
            return new TrainerDetailsViewModel();
        }

        var trainer = new TrainerDetailsViewModel
        {
            Id = firstLine.Id,
            Biography = firstLine.Biography,
            FirstName = firstLine.FirstName,
            LastName = firstLine.LastName,
            Title = firstLine.Title,
            //ProfileImagePath = firstLine.ProfileImagePath,
            SocialNetworks = trainerDetails.ToTrainerSocialNetworks(),
            Trainings = trainerTrainingList.ToTrainingListViewModels()!
        };
        return trainer;
    }

    public static List<TrainerSocialNetwork>? ToTrainerSocialNetworks(this IEnumerable<TrainerDetails>? trainerDetails)
    {
        if (trainerDetails is null)
        {
            return null;
        }

        var socialNetworks = new List<TrainerSocialNetwork>();
        foreach (var detail in trainerDetails.Where(detail => detail.SocialNetwork is not null)
                     .OrderByDescending(s => s.SocialNetwork))
        {
            var socialNetworkName = SocialNetwork.FromValue<SocialNetwork>((int)detail.SocialNetwork!);
            socialNetworks.Add(new TrainerSocialNetwork()
            {
                SocialNetwork = socialNetworkName,
                SocialNetworkUrl = detail.UrlToProfile,
                IconPathFileName = $"/icons/{socialNetworkName}.svg"
            });
        }
        return socialNetworks;
    }
}
