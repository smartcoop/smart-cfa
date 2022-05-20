using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeDetective;
using MimeDetective.Definitions;
using Smart.FA.Catalog.Application.SeedWork;
using Smart.FA.Catalog.Core.Services;
using Smart.FA.Catalog.Infrastructure.Helpers;
using Smart.FA.Catalog.Infrastructure.Persistence;
using Smart.FA.Catalog.Infrastructure.Services.Options;

namespace Smart.FA.Catalog.Application.UseCases.Commands;

public class UploadTrainerProfileImageCommand : IRequestHandler<UploadTrainerProfileImageToStorageCommandRequest, UploadTrainerProfileImageToStorageCommandResponse>
{
    private readonly ILogger<UploadTrainerProfileImageCommand> _logger;
    private readonly CatalogContext _catalogContext;
    private readonly IS3StorageService _storageService;
    private readonly IMinIoLinkGenerator _minIoLinkGenerator;

    public UploadTrainerProfileImageCommand(ILogger<UploadTrainerProfileImageCommand> logger, CatalogContext catalogContext, IS3StorageService storageService, IMinIoLinkGenerator minIoLinkGenerator)
    {
        _logger = logger;
        _catalogContext = catalogContext;
        _storageService = storageService;
        _minIoLinkGenerator = minIoLinkGenerator;
    }

    /// <summary>
    /// Upload a profile picture for the  <see cref="command.Trainer"/> on the S3 Storage server.
    /// If an profile picture already exists for the trainer, it will delete it and replace it with the new one (and save the changed url to db).
    /// </summary>
    /// <param name="command.ProfilePicture">The raw file</param>
    /// <param name="command.Trainer">The trainer whose picture needs to be modified</param>
    /// <param name="cancellationToken">The cancellation token</param>
    public async Task<UploadTrainerProfileImageToStorageCommandResponse> Handle(UploadTrainerProfileImageToStorageCommandRequest command,
        CancellationToken cancellationToken)
    {
        UploadTrainerProfileImageToStorageCommandResponse resp = new();

        var trainer = await _catalogContext.Trainers.FirstAsync(trainer => trainer.Id == command.TrainerId, cancellationToken);

        // If the trainer has already uploaded an image, it is deleted
        if (trainer.ProfileImagePath is not null)
        {
            await _storageService.DeleteAsync(trainer.ProfileImagePath, cancellationToken);
        }

        //Generate new random image name with the correct path
        var profilePictureName = new FileInfo(command.ProfilePicture.FileName);
        var profilePictureUrl = _minIoLinkGenerator.GenerateTrainerProfilePictureUrl(command.TrainerId, profilePictureName.Extension);
        var profilePictureStream = command.ProfilePicture.OpenReadStream();
        await _storageService.UploadAsync(profilePictureStream, profilePictureUrl, cancellationToken);

        //Update relative profile picture path to db
        trainer.UpdateProfileImagePath(profilePictureUrl);
        await _catalogContext.SaveChangesAsync(cancellationToken);

        resp.ProfilePictureAbsoluteUrl = _minIoLinkGenerator.GetAbsoluteTrainerProfilePictureUrl(profilePictureUrl);
        resp.SetSuccess();
        return resp;
    }
}

public class UploadTrainerProfileImageToStorageCommandRequest : IRequest<UploadTrainerProfileImageToStorageCommandResponse>
{
    public IFormFile ProfilePicture { get; set; } = null!;
    public int TrainerId { get; set; }
}

public class UploadTrainerProfileImageToStorageCommandResponse : ResponseBase
{
    public string ProfilePictureAbsoluteUrl { get; set; } = null!;
}

/// <summary>
/// Validates a <see cref="EditProfileCommand" />.
/// </summary>
public class UploadTrainerProfileImageToStorageCommandValidator : AbstractValidator<UploadTrainerProfileImageToStorageCommandRequest>
{
    private readonly IOptions<S3StorageOptions> _storageOptions;

    public UploadTrainerProfileImageToStorageCommandValidator(IOptions<S3StorageOptions> storageOptions)
    {
        _storageOptions = storageOptions;

        RuleFor(request => request.ProfilePicture)
            .Cascade(CascadeMode.Stop)
            .Must(IsUnderMaxSize).WithMessage(CatalogResources.ProfilePage_Image_FileTooBig)
            .MustAsync(IsCorrectTypeAsync).WithMessage(CatalogResources.ProfilePage_Image_WrongFileType);
    }

    private async Task<bool> IsCorrectTypeAsync(IFormFile file, CancellationToken cancellationToken)
    {
        var mimeInspector = new ContentInspectorBuilder { Definitions = Default.FileTypes.Images.All() }.Build();

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
