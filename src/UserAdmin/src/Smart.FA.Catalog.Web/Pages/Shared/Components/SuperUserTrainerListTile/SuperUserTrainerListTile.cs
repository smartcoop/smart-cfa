using MediatR;
using Microsoft.AspNetCore.Mvc;
using Smart.FA.Catalog.Application.UseCases.Queries;
using Smart.FA.Catalog.Core.Domain;
using Smart.FA.Catalog.Core.Domain.User.Enumerations;
using Smart.FA.Catalog.Core.Domain.ValueObjects;

namespace Smart.FA.Catalog.Web.Pages.Shared.Components.SuperUserTrainerListTile;

public class SuperUserTrainerListTile : ViewComponent
{
    private readonly IMediator _mediator;

    public SuperUserTrainerListTile(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IViewComponentResult> InvokeAsync(Trainer trainer)
    {
        var isTrainerBlackListed = await IsBlackListed(trainer);

        var trainerListTile = new TrainerListTile
        {
            Email = trainer.Email!,
            Id = trainer.Id,
            Name = trainer.Name,
            ApplicationType = ApplicationType.FromValue(trainer.Identity.ApplicationTypeId).Name,
            UserId = trainer.Identity.UserId,
            SocialNetworkDictionary = DictionaryOutOfSocialNetwork(trainer.SocialNetworks),
            IsBlackListed = isTrainerBlackListed
        };
        return View(trainerListTile);
    }

    public async Task<bool> IsBlackListed(Trainer trainer) => await _mediator.Send(new IsTrainerBlackListedRequest { TrainerId = trainer.Id });

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

    public bool IsBlackListed { get; set; }
}
