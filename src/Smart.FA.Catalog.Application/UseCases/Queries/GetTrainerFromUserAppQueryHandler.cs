using Application.SeedWork;
using Core.Domain;
using Core.Domain.Enumerations;
using Core.SeedWork;
using Core.Services;
using Infrastructure.Persistence;
using Infrastructure.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Queries;

public class GetTrainerFromUserAppQueryHandler : IRequestHandler<GetTrainerFromUserAppRequest, GetTrainerFromUserAppResponse>
{
    private readonly ILogger<GetTrainerFromUserAppQueryHandler> _logger;
    private readonly UserStrategyResolver _userStrategyResolver;
    private readonly CatalogContext _catalogContext;

    public GetTrainerFromUserAppQueryHandler(ILogger<GetTrainerFromUserAppQueryHandler> logger,
        UserStrategyResolver userStrategyResolver, CatalogContext catalogContext)
    {
        _logger = logger;
        _userStrategyResolver = userStrategyResolver;
        _catalogContext = catalogContext;
    }

    public async Task<GetTrainerFromUserAppResponse> Handle(GetTrainerFromUserAppRequest request, CancellationToken cancellationToken)
    {
        GetTrainerFromUserAppResponse resp = new();
        try
        {
            var userStrategy =  _userStrategyResolver.Resolve(request.ApplicationType!);
            var user = await userStrategy.GetAsync(request.UserId!);
            var linkedTrainer =
                await _catalogContext.Trainers.FirstOrDefaultAsync(trainer => trainer.Identity.UserId == request.UserId, cancellationToken);
            if (linkedTrainer is null)
            {
                linkedTrainer = new Trainer(Name.Create(user.FirstName, user.LastName).Value,
                    TrainerIdentity.Create(user.UserId, Enumeration.FromDisplayName<ApplicationType>(user.ApplicationType)).Value,string.Empty, string.Empty,
                    Language.Create("EN").Value);
                await _catalogContext.Trainers.AddAsync(linkedTrainer, cancellationToken);
                await _catalogContext.SaveChangesAsync(cancellationToken);
            }

            resp.Trainer = linkedTrainer;
            resp.SetSuccess();
        }
        catch (Exception e)
        {
            _logger.LogError(e.ToString());
            throw;
        }
        return resp;
    }
}

public class GetTrainerFromUserAppRequest : IRequest<GetTrainerFromUserAppResponse>
{
    public string UserId { get; init; } = null!;
    public ApplicationType ApplicationType { get; init; } = null!;
}

public class GetTrainerFromUserAppResponse : ResponseBase
{
    public Trainer? Trainer { get; set; }
}
