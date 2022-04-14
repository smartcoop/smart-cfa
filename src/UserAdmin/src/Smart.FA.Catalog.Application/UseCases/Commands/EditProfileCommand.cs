using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeDetective;
using MimeDetective.Definitions;
using Smart.FA.Catalog.Application.Extensions;
using Smart.FA.Catalog.Application.Extensions.FluentValidation;
using Smart.FA.Catalog.Application.SeedWork;
using Smart.FA.Catalog.Core.Domain;
using Smart.FA.Catalog.Core.Extensions;
using Smart.FA.Catalog.Infrastructure.Persistence;
using Smart.FA.Catalog.Infrastructure.Services.Options;
using Smart.FA.Catalog.Shared.Domain.Enumerations;
using Smart.FA.Catalog.Shared.Domain.Enumerations.Trainer;

namespace Smart.FA.Catalog.Application.UseCases.Commands;

/// <summary>
/// Command to edit the profile of a trainer.
/// </summary>
public class EditProfileCommand : IRequest<ProfileEditionResponse>
{
    public int TrainerId { get; set; }

    public string? Bio { get; set; }

    public string? Title { get; set; }

    public Dictionary<string, string>? Socials { get; set; }

    public IFormFile? ProfilePicture { get; set; }

    public string? Email { get; set; }
}

public class EditProfileCommandHandler : IRequestHandler<EditProfileCommand, ProfileEditionResponse>
{
    private readonly ILogger<EditProfileCommandHandler> _logger;
    private readonly CatalogContext _catalogContext;

    public EditProfileCommandHandler(ILogger<EditProfileCommandHandler> logger, CatalogContext catalogContext)
    {
        _logger         = logger;
        _catalogContext = catalogContext;
    }

    public async Task<ProfileEditionResponse> Handle(EditProfileCommand command, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Editing profile of trainer {trainerId}", command.TrainerId);
            var trainer = await GetTrainerWithSocialNetworksByIdAsync(command.TrainerId, cancellationToken);

            // If null is returned this means that the trainer was not found.
            if (trainer is null)
            {
                return new TrainerNotFoundResponse(command.TrainerId);
            }

            UpdateTrainerData(trainer, command);

            // This could have been pulled into the UpdateTrainerData method but I prefer to make it explicit.
            await _catalogContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Profile edition of trainer {trainerId} succeeded", trainer.Id);

            return new TrainerProfileUpdatedSuccessfullyResponse();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "An unexpected error occurred during the executing of the command: {payload}", command.ToJson());
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
            .Include(trainer => trainer.PersonalSocialNetworks)
            .FirstOrDefaultAsync(trainer => trainer.Id == trainerId, cancellationToken);
    }

    /// <summary>
    /// Updates a given <see cref="Trainer" />'s <see cref="Trainer.Biography" />, <see cref="Trainer.Title" /> and <see cref="PersonalSocialNetwork"/>'s.
    /// </summary>
    /// <param name="trainer">The trainer to update.</param>
    /// <param name="command">The command related to the operation.</param>
    private void UpdateTrainerData(Trainer trainer, EditProfileCommand command)
    {
        trainer.UpdateBiography(command.Bio ?? string.Empty);
        trainer.UpdateTitle(command.Title ?? string.Empty);
        if (command.ProfilePicture is not null)
        {
            trainer.UpdateProfileImagePath(trainer.GenerateTrainerProfilePictureName());
        }

        trainer.ChangeEmail(command.Email);
        foreach (var commandSocial in command.Socials!)
        {
            var socialNetwork = Enumeration.FromValue<SocialNetwork>(int.Parse(commandSocial.Key));
            var url = commandSocial.Value;
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

        RuleFor(command => command.Email)
            .ValidEmail();

        When(request => request.ProfilePicture is not null,
            () => RuleFor(request => request.ProfilePicture!)
                .Cascade(CascadeMode.Stop)
                .Must(IsUnderMaxSize).WithMessage(CatalogResources.ProfilePage_Image_FileTooBig)
                .MustAsync(IsCorrectTypeAsync).WithMessage(CatalogResources.ProfilePage_Image_WrongFileType));
    }

    private async Task<bool> IsCorrectTypeAsync(IFormFile file, CancellationToken cancellationToken)
    {
        var mimeInspector = new ContentInspectorBuilder {Definitions = Default.FileTypes.Images.All()}.Build();

        var fileStream = file.OpenReadStream();
        MemoryStream memoryStream = new();
        await fileStream.CopyToAsync(memoryStream, cancellationToken);
        var result = mimeInspector.Inspect(memoryStream.ToArray());
        return !result.IsDefaultOrEmpty;
    }

    private bool IsUnderMaxSize(IFormFile file)
    {
        return file.Length < _storageOptions.Value.FileSizeLimit;
    }
}
