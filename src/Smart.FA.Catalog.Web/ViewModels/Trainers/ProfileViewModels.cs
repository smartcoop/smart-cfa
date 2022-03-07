using Application.UseCases.Commands;
using Application.UseCases.Queries;
using Core.Domain;
using Core.SeedWork;
using Smart.Design.Razor.TagHelpers.Icon;

namespace Web.ViewModels.Trainers;

public class SocialNetworkViewModel
{
    public int SocialNetworkId { get; set; }

    public string? Url { get; set; }

    public string? Name { get; set; }

    public Image Icon { get; set; }
}

public static class Mappers
{
    public static EditProfileCommand ToCommand(this TrainerProfile trainerProfile)
    {
        return new EditProfileCommand()
        {
            TrainerId = trainerProfile.TrainerId.Value,
            Bio       = trainerProfile.Bio,
            Title     = trainerProfile.Title
        };
    }

    /// <summary>
    /// Maps a collection of <see cref="TrainerProfile.Social"/> to a collection of <see cref="SocialNetworkViewModel" />.
    /// </summary>
    /// <param name="trainerSocials">Current registered social media of a trainer.</param>
    /// <returns>A given trainer's personal social networks.</returns>
    public static ICollection<SocialNetworkViewModel> ToSocialViewModels(this IEnumerable<TrainerProfile.Social> trainerSocials)
    {
        // Retrieve enumeration of all social networks in our system.
        IDictionary<int, SocialNetworkViewModel> socials = Enumeration.GetAll<SocialNetwork>()
            .Select(socialNetwork => socialNetwork.ToViewModel())
            .ToDictionary(socialVm => socialVm.SocialNetworkId, value => value);

        // Restore user social network profile url now
        foreach (var dataSocial in trainerSocials)
        {
            socials[dataSocial.SocialNetworkId].Url = dataSocial.Url;
        }

        return socials.Values;
    }

    public static SocialNetworkViewModel ToViewModel(this SocialNetwork socialNetwork)
    {
        return new SocialNetworkViewModel()
        {
            SocialNetworkId = socialNetwork.Id,
            Name            = socialNetwork.Name,
            Icon            = socialNetwork.ToImage()
        };
    }

    /// <summary>
    /// Returns the logo of a given <see cref="SocialNetwork" />.
    /// If no logo is found for the given <paramref name="socialNetwork" /> than <see cref="Image.None" /> is returned.
    /// </summary>
    /// <param name="socialNetwork">A social network whose logo needs to be returned.</param>
    /// <returns>The logo of a social media.</returns>
    public static Image ToImage(this SocialNetwork socialNetwork)
    {
        var icon = Image.None;
        try
        {
            icon = Enum.Parse<Image>(socialNetwork.Name, ignoreCase: true);
        }
        catch
        {
            // ignored
        }

        return icon;
    }
}
