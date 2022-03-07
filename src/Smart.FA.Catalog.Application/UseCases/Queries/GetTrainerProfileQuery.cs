using Core.Domain;
using Core.Extensions;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Queries;

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
    private readonly CatalogContext _catalogContext;

    /// <summary>
    /// Represents the response of the query when the trainer was not found.
    /// <see cref="TrainerProfile.TrainerId" /> is a <see cref="Nullable" />.
    /// </summary>
    private static TrainerProfile TrainerNotFoundResponse => new();

    public GetTrainerProfileQueryHandler(ILogger<GetTrainerProfileQueryHandler> logger, CatalogContext catalogContext)
    {
        _logger         = logger;
        _catalogContext = catalogContext;
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
            .Include(trainer => trainer.PersonalSocialNetworks)
            .Select(trainer => new
            {
                TrainerId = trainer.Id,
                Bio       = trainer.Biography,
                Name      = trainer.Name.FirstName + " " + trainer.Name.LastName,
                Title     = trainer.Title,
                Socials   = trainer.PersonalSocialNetworks
            })
            .FirstOrDefaultAsync(trainer => trainer.TrainerId == query.TrainerId, cancellationToken);

        return trainer is not null
            ? new TrainerProfile()
            {
                TrainerId = trainer.TrainerId,
                Bio       = trainer.Bio,
                Name      = trainer.Name,
                Title     = trainer.Title,
                Socials   = trainer.Socials.ToTrainerProfileSocials()
            }
            : null;
    }
}

public class TrainerProfile
{
    public int? TrainerId { get; set; }

    public string? Bio { get; set; }

    public string? Name { get; set; }

    public string? Title { get; set; }

    public IEnumerable<Social>? Socials { get; set; }

    public class Social
    {
        public int SocialNetworkId { get; set; }

        public string? Url { get; set; }
    }
}

public static class Mappers
{
    public static IEnumerable<TrainerProfile.Social> ToTrainerProfileSocials(this IEnumerable<PersonalSocialNetwork> personalSocialNetworks)
    {
        return personalSocialNetworks.Select(socialNetwork => new TrainerProfile.Social()
        {
            SocialNetworkId = socialNetwork.SocialNetwork.Id,
            Url             = socialNetwork.UrlToProfile
        });
    }
}
