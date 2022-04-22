using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Smart.FA.Catalog.Core.Domain;
using Smart.FA.Catalog.Core.Extensions;
using Smart.FA.Catalog.Core.Services;
using Smart.FA.Catalog.Infrastructure.Persistence;

namespace Smart.FA.Catalog.Application.UseCases.Queries;

/// <summary>
/// Query returning the necessary data to render the profile page of a <see cref="Trainer"/>.
/// </summary>
public class GetTrainerProfileQuery : IRequest<TrainerProfile>
{
    public int TrainerId { get; set; }

    public GetTrainerProfileQuery(int id)
    {
        TrainerId = id;
    }
}

public class GetTrainerProfileQueryHandler : IRequestHandler<GetTrainerProfileQuery, TrainerProfile>
{
    private readonly ILogger<GetTrainerProfileQueryHandler> _logger;
    private readonly CatalogContext                         _catalogContext;
    private readonly IS3StorageService                      _storageService;

    /// <summary>
    /// Represents the response of the query when the trainer was not found.
    /// <see cref="TrainerProfile.TrainerId" /> is a <see cref="Nullable" />.
    /// </summary>
    private static TrainerProfile TrainerNotFoundResponse => new();

    public GetTrainerProfileQueryHandler
    (
        ILogger<GetTrainerProfileQueryHandler> logger,
        CatalogContext                         catalogContext,
        IS3StorageService                      storageService
    )
    {
        _logger         = logger;
        _catalogContext = catalogContext;
        _storageService = storageService;
    }

    public async Task<TrainerProfile> Handle(GetTrainerProfileQuery query, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Fetching trainer {id} profile data", query.TrainerId);
            var trainerProfileData = await GetTrainerProfileAsync(query, cancellationToken);
            return trainerProfileData ?? TrainerNotFoundResponse;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, $"An unexpected error occurred while fetch trainer profile {query.TrainerId}. {query.ToJson()}");
            throw;
        }
    }

    private async Task<TrainerProfile?> GetTrainerProfileAsync(GetTrainerProfileQuery query, CancellationToken cancellationToken)
    {
        var trainer = await _catalogContext.Trainers
            .Include(trainer => trainer.SocialNetworks)
            .Select(trainer => new
            {
                TrainerId        = trainer.Id,
                Bio              = trainer.Biography,
                Name             = trainer.Name.FirstName + " " + trainer.Name.LastName,
                Title            = trainer.Title,
                Socials          = trainer.SocialNetworks,
                ProfileImagePath = trainer.ProfileImagePath,
                Email            = trainer.Email
            })
            .FirstOrDefaultAsync(trainer => trainer.TrainerId == query.TrainerId, cancellationToken);


        if (trainer is null)
        {
            return null;
        }

        Stream? profileImage = null;
        if (trainer.ProfileImagePath is not null)
        {
            profileImage = await _storageService.GetAsync(trainer.ProfileImagePath, cancellationToken);
        }

        return new TrainerProfile
        {
            TrainerId    = trainer.TrainerId,
            Bio          = trainer.Bio,
            Name         = trainer.Name,
            Title        = trainer.Title,
            Socials      = trainer.Socials.ToTrainerProfileSocials(),
            ProfileImage = profileImage,
            Email        = trainer.Email
        };
    }
}

public class TrainerProfile
{
    public int? TrainerId { get; set; }

    public string? Bio { get; set; }

    public string? Name { get; set; }

    public string? Title { get; set; }

    public string? Email { get; set; }

    public IEnumerable<Social> Socials { get; set; } = null!;

    public class Social
    {
        public int SocialNetworkId { get; set; }

        public string? Url { get; set; }
    }

    public Stream? ProfileImage { get; set; }
}

public static class Mappers
{
    public static IEnumerable<TrainerProfile.Social> ToTrainerProfileSocials(this IEnumerable<TrainerSocialNetwork> socialNetworks)
    {
        return socialNetworks.Select(socialNetwork => new TrainerProfile.Social() { SocialNetworkId = socialNetwork.SocialNetwork.Id, Url = socialNetwork.UrlToProfile });
    }
}
