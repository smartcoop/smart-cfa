using MediatR;
using Smart.FA.Catalog.Application.SeedWork;
using Smart.FA.Catalog.Core.Services;
using Smart.FA.Catalog.Infrastructure.Helpers;

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
            await _storageService.DeleteAsync(_minIoLinkGenerator.GetAbsoluteTrainerProfilePictureUrl(command.RelativeProfilePictureUrl), cancellationToken);
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
