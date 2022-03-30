using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Smart.FA.Catalog.Application.Extensions;
using Smart.FA.Catalog.Application.SeedWork;
using Smart.FA.Catalog.Core.Domain;
using Smart.FA.Catalog.Core.Services;

namespace Smart.FA.Catalog.Application.UseCases.Commands;

public class UploadTrainerProfileImageCommand : IRequestHandler<UploadImageToStorageCommandRequest, UploadImageToStorageCommandResponse>
{
    private readonly ILogger<UploadTrainerProfileImageCommand> _logger;
    private readonly IS3StorageService _storageService;

    public UploadTrainerProfileImageCommand(ILogger<UploadTrainerProfileImageCommand> logger, IS3StorageService storageService)
    {
        _logger = logger;
        _storageService = storageService;
    }

    /// <summary>
    /// Upload a new one with a name based on id the <see cref="command.Trainer"/> on the S3 Storage server.
    /// If the image already exists, it will delete it and replace it with the new one.
    /// </summary>
    /// <param name="command.ProfilePicture">The raw file</param>
    /// <param name="command.Trainer">The trainer whose picture needs to be modified</param>
    /// <param name="cancellationToken">The cancellation token</param>
    public async Task<UploadImageToStorageCommandResponse> Handle(UploadImageToStorageCommandRequest command,
        CancellationToken cancellationToken)
    {
        UploadImageToStorageCommandResponse resp = new();

        if (command.Trainer.ProfileImagePath is not null)
        {
            await _storageService.DeleteAsync(command.Trainer.ProfileImagePath, cancellationToken);
        }

        var newFileName = command.Trainer.GenerateTrainerProfilePictureName();
        var fileStream = command.ProfilePicture.OpenReadStream();
        await _storageService.UploadAsync(fileStream, newFileName, cancellationToken);
        resp.ProfilePictureStream = fileStream;
        resp.SetSuccess();

        return resp;
    }
}

public class UploadImageToStorageCommandRequest : IRequest<UploadImageToStorageCommandResponse>
{
    public IFormFile ProfilePicture { get; set; } = null!;
    public Trainer Trainer { get; set; } = null!;
}

public class UploadImageToStorageCommandResponse : ResponseBase
{
    public Stream ProfilePictureStream { get; set; } = null!;
}
