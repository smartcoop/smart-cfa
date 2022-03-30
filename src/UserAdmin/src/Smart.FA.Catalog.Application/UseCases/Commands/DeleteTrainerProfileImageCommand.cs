using MediatR;
using Smart.FA.Catalog.Application.Extensions;
using Smart.FA.Catalog.Application.SeedWork;
using Smart.FA.Catalog.Core.Domain;
using Smart.FA.Catalog.Core.Services;

namespace Smart.FA.Catalog.Application.UseCases.Commands;

public class DeleteTrainerProfileImageCommand : IRequestHandler<DeleteTrainerProfileImageRequest, DeleteTrainerProfileImageResponse>
{
    private readonly IS3StorageService _storageService;

    public DeleteTrainerProfileImageCommand(IS3StorageService storageService)
    {
        _storageService = storageService;
    }

    public async Task<DeleteTrainerProfileImageResponse> Handle(DeleteTrainerProfileImageRequest command, CancellationToken cancellationToken)
    {
        DeleteTrainerProfileImageResponse response = new();

        await _storageService.DeleteAsync(command.Trainer.GenerateTrainerProfilePictureName(), cancellationToken);
        response.SetSuccess();

        return response;
    }
}

public class DeleteTrainerProfileImageRequest : IRequest<DeleteTrainerProfileImageResponse>
{
    public Trainer Trainer { get; set; } = null!;
}

public class DeleteTrainerProfileImageResponse : ResponseBase
{
}
