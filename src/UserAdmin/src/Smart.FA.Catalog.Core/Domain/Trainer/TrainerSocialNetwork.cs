using Smart.FA.Catalog.Core.Exceptions;
using Smart.FA.Catalog.Core.SeedWork;
using Smart.FA.Catalog.Shared.Collections;
using Smart.FA.Catalog.Shared.Domain.Enumerations.Trainer;

namespace Smart.FA.Catalog.Core.Domain;

/// <summary>
/// Represents a trainer's social network URL to its profile.
/// </summary>
public class TrainerSocialNetwork
{
    public int TrainerId { get; private set; }

    public string? UrlToProfile { get; private set; }

    public SocialNetwork SocialNetwork { get; private set; } = null!;

    public virtual Trainer? Trainer { get; private set; }

    protected TrainerSocialNetwork()
    {

    }

    public TrainerSocialNetwork(int trainerId, SocialNetwork socialNetwork, string? urlToSocialNetworkProfile)
    {
        SetSocialNetworkInfo(trainerId, socialNetwork, urlToSocialNetworkProfile);
    }

    public void SetSocialNetworkInfo(int trainerId, SocialNetwork? socialNetwork, string? urlToSocialNetworkProfile)
    {
        Guard.Requires(() => trainerId != 0, Errors.Trainer.TrainerIsTransient().Message);
        Guard.AgainstNull(socialNetwork, nameof(socialNetwork));

        TrainerId     = trainerId;
        SocialNetwork = socialNetwork!;
        UrlToProfile  = urlToSocialNetworkProfile;
    }
}
