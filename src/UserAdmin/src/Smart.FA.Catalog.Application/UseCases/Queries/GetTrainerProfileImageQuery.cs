using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Smart.FA.Catalog.Application.Extensions;
using Smart.FA.Catalog.Application.SeedWork;
using Smart.FA.Catalog.Core.Domain;
using Smart.FA.Catalog.Core.Services;
using Smart.FA.Catalog.Infrastructure.Services.Options;

namespace Smart.FA.Catalog.Application.UseCases.Queries;

public class GetTrainerProfileImageQuery : IRequestHandler<GetTrainerProfileImageRequest, GetTrainerProfileImageResponse>
{
    private readonly ILogger<GetTrainerProfileImageQuery> _logger;
    private readonly IS3StorageService _storageService;
    private readonly S3StorageOptions _storageOptions;

    public GetTrainerProfileImageQuery
    (
        ILogger<GetTrainerProfileImageQuery> logger,
        IS3StorageService storageService,
        IOptions<S3StorageOptions> storageOptions)
    {
        _logger = logger;
        _storageService = storageService;
        _storageOptions = storageOptions.Value;
    }

    public async Task<GetTrainerProfileImageResponse> Handle(GetTrainerProfileImageRequest query, CancellationToken cancellationToken)
    {
        GetTrainerProfileImageResponse response = new();

        //In case the trainer has yet to upload his profile picture, a default image is served on his profile page.
        response.ImageStream = query.Trainer.ProfileImagePath is null
            ? await _storageService.GetAsync(_storageOptions.DefaultTrainerProfilePictureName, cancellationToken)
            : await _storageService.GetAsync(query.Trainer.ProfileImagePath, cancellationToken);

        response.SetSuccess();
        return response;
    }
}

public class GetTrainerProfileImageRequest : IRequest<GetTrainerProfileImageResponse>
{
    public Trainer Trainer { get; set; } = null!;
}

public class GetTrainerProfileImageResponse : ResponseBase
{
    public Stream? ImageStream { get; set; }
}
