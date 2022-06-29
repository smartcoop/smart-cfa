using Microsoft.AspNetCore.Mvc;
using Smart.FA.Catalog.Core.Domain;
using Smart.FA.Catalog.Core.Domain.User.Enumerations;
using Smart.FA.Catalog.Core.Domain.ValueObjects;

namespace Smart.FA.Catalog.Web.Pages.Shared.Components.SuperUserTrainerListTile;

public class SuperUserTrainerListTile : ViewComponent
{
    public IViewComponentResult Invoke(Trainer trainer)
    {
        var trainerListTile = new TrainerListTile
        {
            Email = trainer.Email!,
            Id = trainer.Id,
            Name = trainer.Name,
            ApplicationType = ApplicationType.FromValue(trainer.Identity.ApplicationTypeId).Name,
            UserId = trainer.Identity.UserId,
            SocialNetworkDictionary = DictionaryOutOfSocialNetwork(trainer.SocialNetworks)
        };
        return View(trainerListTile);
    }

    public Dictionary<string, string> DictionaryOutOfSocialNetwork(IEnumerable<TrainerSocialNetwork> trainerSocialNetworks)
        => trainerSocialNetworks
            .Where(trainerSocialNetwork => !string.IsNullOrEmpty(trainerSocialNetwork.UrlToProfile))
            .ToDictionary(trainerSocialNetwork => trainerSocialNetwork.SocialNetwork.Name, trainerSocialNetwork => trainerSocialNetwork.UrlToProfile!);
}

public class TrainerListTile
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public Name Name { get; set; } = null!;

    public Dictionary<string, string> SocialNetworkDictionary { get; set; } = new();

    public string UserId { get; set; } = null!;

    public string ApplicationType { get; set; } = null!;
}
