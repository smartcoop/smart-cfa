using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Smart.FA.Catalog.Application.SeedWork;
using Smart.FA.Catalog.Core.Domain;
using Smart.FA.Catalog.Core.Extensions;
using Smart.FA.Catalog.Infrastructure.Helpers;
using Smart.FA.Catalog.Infrastructure.Persistence;
using Smart.FA.Catalog.Infrastructure.Services.Options;
using Smart.FA.Catalog.Shared.Domain.Enumerations.Trainer;
using Smart.FA.Catalog.Shared.Helper;

namespace Smart.FA.Catalog.Application.UseCases.Commands;

/// <summary>
/// Command to edit the profile of a trainer.
/// </summary>
public class EditProfileCommand : IRequest<ProfileEditionResponse>
{
    public int TrainerId { get; set; }

    public string? Bio { get; set; }

    public string? Title { get; set; }

    public Dictionary<int, string>? Socials { get; set; }
}

public class EditProfileCommandHandler : IRequestHandler<EditProfileCommand, ProfileEditionResponse>
{
    private readonly ILogger<EditProfileCommandHandler> _logger;
    private readonly CatalogContext _catalogContext;
    private readonly IMinIoLinkGenerator _minIoLinkGenerator;

    public EditProfileCommandHandler(ILogger<EditProfileCommandHandler> logger, CatalogContext catalogContext, IMinIoLinkGenerator minIoLinkGenerator)
    {
        _logger = logger;
        _catalogContext = catalogContext;
        _minIoLinkGenerator = minIoLinkGenerator;
    }

    public async Task<ProfileEditionResponse> Handle(EditProfileCommand command, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Editing profile of trainer {TrainerId}", command.TrainerId);
            var trainer = await GetTrainerWithSocialNetworksByIdAsync(command.TrainerId, cancellationToken);

            // If null is returned this means that the trainer was not found.
            if (trainer is null)
            {
                return new TrainerNotFoundResponse(command.TrainerId);
            }

            UpdateTrainerData(trainer, command);

            // This could have been pulled into the UpdateTrainerData method but I prefer to make it explicit.
            await _catalogContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Profile edition of trainer {TrainerId} succeeded", trainer.Id);

            return new TrainerProfileUpdatedSuccessfullyResponse();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "An unexpected error occurred during the executing of the command: {Payload}", command.ToJson());
            throw;
        }
    }

    /// <summary>
    /// Returns a <see cref="Trainer" /> with its social networks.
    /// </summary>
    /// <param name="trainerId">The id of the trainer to be retrieved.</param>
    /// <param name="cancellationToken">A token to cancel the current execution of the process.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains an instance of a <see cref="Trainer" />.
    /// If no <see cref="Trainer" /> is found then the task result is null.
    /// </returns>
    private Task<Trainer?> GetTrainerWithSocialNetworksByIdAsync(int trainerId, CancellationToken cancellationToken)
    {
        return _catalogContext.Trainers
            .Include(trainer => trainer.SocialNetworks)
            .FirstOrDefaultAsync(trainer => trainer.Id == trainerId, cancellationToken);
    }

    /// <summary>
    /// Updates a given <see cref="Trainer" />'s <see cref="Trainer.Biography" />, <see cref="Trainer.Title" /> and <see cref="TrainerSocialNetwork"/>'s.
    /// </summary>
    /// <param name="trainer">The trainer to update.</param>
    /// <param name="command">The command related to the operation.</param>
    private void UpdateTrainerData(Trainer trainer, EditProfileCommand command)
    {
        trainer.UpdateBiography(command.Bio ?? string.Empty);
        trainer.UpdateTitle(command.Title ?? string.Empty);
        foreach (var commandSocial in command.Socials!)
        {
            var socialNetwork = SocialNetwork.FromValue(commandSocial.Key);
            var url = commandSocial.Value;
            if (!string.IsNullOrEmpty(url) &&
                !url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
                !url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                url = "https://" + url;
            }

            trainer.SetSocialNetwork(socialNetwork, url);
        }
    }
}

// ---------------------------------------
// --- EditProfileCommand's responses. ---
// ---------------------------------------

/// <summary>
/// Represents a <see cref="ProfileEditionResponse" /> when the edition completed successfully.
/// </summary>
public class TrainerProfileUpdatedSuccessfullyResponse : ProfileEditionResponse
{
    public TrainerProfileUpdatedSuccessfullyResponse()
    {
        Found = true;
        SetSuccess();
    }
}

/// <summary>
/// Represents a <see cref="ProfileEditionResponse" /> where the trainer was not found during the command execution.
/// </summary>
public class TrainerNotFoundResponse : ProfileEditionResponse
{
    public TrainerNotFoundResponse(int trainerId)
    {
        Found = false;
        AddError("TrainerNotFound", CatalogResources.TrainerNotFound_TrainerId);
    }
}

/// <summary>
/// Base response class for the edition of a <see cref="Trainer" />'s profile.
/// </summary>
public class ProfileEditionResponse : ResponseBase
{
    public bool Found { get; set; }
}

/// <summary>
/// Validates a <see cref="EditProfileCommand" />.
/// </summary>
public class EditProfileCommandValidator : AbstractValidator<EditProfileCommand>
{
    private readonly IOptions<S3StorageOptions> _storageOptions;

    public EditProfileCommandValidator(IOptions<S3StorageOptions> storageOptions)
    {
        _storageOptions = storageOptions;

        RuleFor(command => command.Bio)
            .MinimumLength(30)
            .WithMessage(CatalogResources.BioMustBe30Chars)
            .MaximumLength(500)
            .WithMessage(CatalogResources.BioCannotExceed500Chars);

        RuleFor(command => command.Socials).SetValidator(new SocialNetworkValidator());
    }

    private sealed class SocialNetworkValidator : AbstractValidator<Dictionary<int, string>?>
    {
        public SocialNetworkValidator()
        {
            RuleForEach(socials => socials)
                .OverrideIndexer((_, _, keyValuePair, _) => $"[{keyValuePair.Key}]")
                .Must(socialNetwork =>
                {
                    var socialUrl = socialNetwork.Value;
                    return string.IsNullOrWhiteSpace(socialUrl) || UriHelper.IsValidUrl(socialUrl);
                });
        }
    }
}
