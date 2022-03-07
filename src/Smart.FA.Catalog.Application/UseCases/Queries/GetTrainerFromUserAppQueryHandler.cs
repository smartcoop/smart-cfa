using Application.SeedWork;
using Core.Domain;
using Core.Domain.Dto;
using Core.Domain.Enumerations;
using Core.SeedWork;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Queries;

public class
    GetTrainerFromUserAppQueryHandler : IRequestHandler<GetTrainerFromUserAppRequest, GetTrainerFromUserAppResponse>
{
    private readonly ILogger<GetTrainerFromUserAppQueryHandler> _logger;
    private readonly CatalogContext _context;

    public GetTrainerFromUserAppQueryHandler(ILogger<GetTrainerFromUserAppQueryHandler> logger, CatalogContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<GetTrainerFromUserAppResponse> Handle(GetTrainerFromUserAppRequest query,
        CancellationToken cancellationToken)
    {
        GetTrainerFromUserAppResponse response = new();
        response.Trainer = await _context.Trainers.FirstOrDefaultAsync(trainer =>
            trainer.Identity.UserId == query.User.UserId && trainer.Identity.ApplicationTypeId ==
            Enumeration.FromDisplayName<ApplicationType>(query.User.ApplicationType).Id, cancellationToken);
        response.SetSuccess();

        return response;
    }
}

public class GetTrainerFromUserAppResponse : ResponseBase
{
    public Trainer? Trainer { get; set; }
}

public class GetTrainerFromUserAppRequest : IRequest<GetTrainerFromUserAppResponse>
{
    public UserDto User { get; set; } = null!;
}
