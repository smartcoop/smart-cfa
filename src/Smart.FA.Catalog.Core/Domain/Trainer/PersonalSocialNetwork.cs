using Core.SeedWork;

namespace Core.Domain;

/// <summary>
/// Represents a trainer's social network URL to its profile.
/// </summary>
public class PersonalSocialNetwork : Entity
{
    public int TrainerId { get; private set; }

    public string? UrlToProfile { get; private set; }

    public SocialNetwork SocialNetwork { get; private set; } = null!;

    public virtual Trainer? Trainer { get; private set; }

    protected PersonalSocialNetwork()
    {

    }

    public PersonalSocialNetwork(int trainerId, SocialNetwork socialNetwork, string? urlToSocialNetworkProfile)
    {
        SetPersonalSocialNetworkInfo(trainerId, socialNetwork, urlToSocialNetworkProfile);
    }

    public void SetPersonalSocialNetworkInfo(int trainerId, SocialNetwork? socialNetwork, string? urlToSocialNetworkProfile)
    {
        Guard.Requires(() => trainerId != 0, "The trainer cannot be transient");
        Guard.AgainstNull(socialNetwork, nameof(socialNetwork));

        TrainerId     = trainerId;
        SocialNetwork = socialNetwork!;
        UrlToProfile  = urlToSocialNetworkProfile;
    }
}
