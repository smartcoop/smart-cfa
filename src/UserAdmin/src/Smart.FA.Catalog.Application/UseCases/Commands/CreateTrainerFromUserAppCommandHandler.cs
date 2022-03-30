using MediatR;
using Microsoft.Extensions.Logging;
using Smart.FA.Catalog.Application.SeedWork;
using Smart.FA.Catalog.Core.Domain;
using Smart.FA.Catalog.Core.Domain.User.Dto;
using Smart.FA.Catalog.Core.Domain.User.Enumerations;
using Smart.FA.Catalog.Core.Domain.ValueObjects;
using Smart.FA.Catalog.Core.LogEvents;
using Smart.FA.Catalog.Core.SeedWork;
using Smart.FA.Catalog.Infrastructure.Persistence;
using Smart.FA.Catalog.Shared.Domain.Enumerations;

namespace Smart.FA.Catalog.Application.UseCases.Commands;

public class
    CreateTrainerFromUserAppCommandHandler : IRequestHandler<CreateTrainerFromUserAppRequest,
        CreateTrainerFromUserAppResponse>
{
    private readonly ILogger<CreateTrainerFromUserAppCommandHandler> _logger;
    private readonly CatalogContext _context;

    public CreateTrainerFromUserAppCommandHandler
    (
        ILogger<CreateTrainerFromUserAppCommandHandler> logger
        , CatalogContext context
    )
    {
        _logger = logger;
        _context = context;
    }

    public async Task<CreateTrainerFromUserAppResponse> Handle(CreateTrainerFromUserAppRequest command,
        CancellationToken cancellationToken)
    {
        CreateTrainerFromUserAppResponse appResponse = new();

        var linkedTrainer = new Trainer
        (
            Name.Create
            (
                command.User.FirstName
                , command.User.LastName
            ).Value
            , TrainerIdentity.Create
            (
                command.User.UserId
                , Enumeration.FromDisplayName<ApplicationType>(command.User.ApplicationType)
            ).Value
            , string.Empty
            , string.Empty
            , Language.Create("EN").Value
        );
        _context.Trainers.Add(linkedTrainer);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(LogEventIds.TrainerCreated,
            "A new trainer for user id {UserId}, application type {ApplicationType} was created", command.User.UserId,
            command.User.ApplicationType);

        appResponse.Trainer = linkedTrainer;

        appResponse.SetSuccess();

        return appResponse;
    }
}

public class CreateTrainerFromUserAppRequest : IRequest<CreateTrainerFromUserAppResponse>
{
    public UserDto User { get; set; } = null!;
}

public class CreateTrainerFromUserAppResponse : ResponseBase
{
    public Trainer Trainer { get; set; } = null!;
}
