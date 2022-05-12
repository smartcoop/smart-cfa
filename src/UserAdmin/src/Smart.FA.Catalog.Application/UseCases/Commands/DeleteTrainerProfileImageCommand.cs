using MediatR;
using Microsoft.Extensions.Options;
using Smart.FA.Catalog.Application.Extensions;
using Smart.FA.Catalog.Application.SeedWork;
using Smart.FA.Catalog.Core.Domain;
using Smart.FA.Catalog.Core.Services;
using Smart.FA.Catalog.Infrastructure.Helpers;
using Smart.FA.Catalog.Infrastructure.Services.Options;

namespace Smart.FA.Catalog.Application.UseCases.Commands;

public class DeleteTrainerProfileImageCommand : IRequestHandler<DeleteTrainerProfileImageRequest, DeleteTrainerProfileImageResponse>
{
    private readonly IS3StorageService _storageService;
    private readonly IMinIoLinkGenerator _minIoLinkGenerator;

    public DeleteTrainerProfileImageCommand(IS3StorageService storageService, IMinIoLinkGenerator minIoLinkGenerator)
    {
        _storageService = storageService;
        _minIoLinkGenerator = minIoLinkGenerator;
    }

    public async Task<DeleteTrainerProfileImageResponse> Handle(DeleteTrainerProfileImageRequest command, CancellationToken cancellationToken)
    {
        DeleteTrainerProfileImageResponse response = new();

        if (command.RelativeProfilePictureUrl is not null)
        {
            await _storageService.DeleteAsync(_minIoLinkGenerator.GetFullTrainerProfilePictureUrl(command.RelativeProfilePictureUrl), cancellationToken);
        }
        response.SetSuccess();
        return response;
    }
}

public class DeleteTrainerProfileImageRequest : IRequest<DeleteTrainerProfileImageResponse>
{
    public string? RelativeProfilePictureUrl { get; set; }
}

public class DeleteTrainerProfileImageResponse : ResponseBase
{
}
