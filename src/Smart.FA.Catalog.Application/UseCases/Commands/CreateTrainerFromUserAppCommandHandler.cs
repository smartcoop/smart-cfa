using Application.SeedWork;
using Core.Domain;
using Core.Domain.Dto;
using Core.Domain.Enumerations;
using Core.LogEvents;
using Core.SeedWork;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Commands;

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
