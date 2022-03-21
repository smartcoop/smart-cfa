using Application.SeedWork;
using Core.Domain;
using Core.Domain.Dto;
using Core.Domain.Enumerations;
using Core.Exceptions;
using Infrastructure.Persistence;
using Infrastructure.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Queries;

public class
    GetTrainerFromUserAppQueryHandler : IRequestHandler<GetTrainerFromUserAppRequest, GetTrainerFromUserAppResponse>
{
    private readonly ILogger<GetTrainerFromUserAppQueryHandler> _logger;
    private readonly CatalogContext _context;
    private readonly UserStrategyResolver _userStrategyResolver;

    public GetTrainerFromUserAppQueryHandler
    (
        ILogger<GetTrainerFromUserAppQueryHandler> logger
        , CatalogContext context
        , UserStrategyResolver userStrategyResolver
    )
    {
        _logger = logger;
        _context = context;
        _userStrategyResolver = userStrategyResolver;
    }

    public async Task<GetTrainerFromUserAppResponse> Handle(GetTrainerFromUserAppRequest query,
        CancellationToken cancellationToken)
    {
        GetTrainerFromUserAppResponse response = new();
        var userStrategy = _userStrategyResolver.Resolve(query.ApplicationType);
        response.User = await userStrategy.GetAsync(query.UserId);

        if (response.User is null) throw new UserException(Errors.User.NotFound(query.UserId));

        response.Trainer = await _context.Trainers.FirstOrDefaultAsync(trainer =>
            trainer.Identity.UserId == query.UserId && trainer.Identity.ApplicationTypeId ==
            query.ApplicationType.Id, cancellationToken);
        response.SetSuccess();

        return response;
    }
}

public class GetTrainerFromUserAppResponse : ResponseBase
{
    public Trainer? Trainer { get; set; }
    public UserDto User { get; set; } = null!;
}

public class GetTrainerFromUserAppRequest : IRequest<GetTrainerFromUserAppResponse>
{
    public string UserId { get; set; } = null!;
    public ApplicationType ApplicationType { get; set; } = null!;
}
