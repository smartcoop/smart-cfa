﻿using MediatR;
using Microsoft.Extensions.Logging;
using Smart.FA.Catalog.Application.SeedWork;
using Smart.FA.Catalog.Core.Domain;
using Smart.FA.Catalog.Core.Services;

namespace Smart.FA.Catalog.Application.UseCases.Queries;

public class GetTrainerProfileImageQuery : IRequestHandler<GetTrainerProfileImageRequest, GetTrainerProfileImageResponse>
{
    private readonly ILogger<GetTrainerProfileImageQuery> _logger;
    private readonly IS3StorageService _storageService;

    public GetTrainerProfileImageQuery
    (
        ILogger<GetTrainerProfileImageQuery> logger,
        IS3StorageService storageService
    )
    {
        _logger = logger;
        _storageService = storageService;
    }

    public async Task<GetTrainerProfileImageResponse> Handle(GetTrainerProfileImageRequest query,
        CancellationToken cancellationToken)
    {
        GetTrainerProfileImageResponse response = new();
        if (query.Trainer.ProfileImagePath is null)
        {
            return response;
        }

        response.ImageStream = await _storageService.GetAsync(query.Trainer.ProfileImagePath, cancellationToken);
        response.SetSuccess();
        return response;
    }
}

public class GetTrainerProfileImageRequest : IRequest<GetTrainerProfileImageResponse>
{
    public Trainer Trainer { get; set; }
}

public class GetTrainerProfileImageResponse : ResponseBase
{
    public Stream? ImageStream { get; set; }
}
