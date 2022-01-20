using Application.SeedWork;
using Core.Domain;
using Core.Domain.Enumerations;
using Core.SeedWork;
using Core.Services;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Queries;

public class GetTrainerFromUserAppQueryHandler : IRequestHandler<GetTrainerFromUserAppRequest, GetTrainerFromUserAppResponse>
{
    private readonly ILogger<GetTrainerFromUserAppQueryHandler> _logger;
    private readonly IUserStrategy _userStrategy;
    private readonly Context _context;

    public GetTrainerFromUserAppQueryHandler(ILogger<GetTrainerFromUserAppQueryHandler> logger,
        IUserStrategy userStrategy, Context context)
    {
        _logger = logger;
        _userStrategy = userStrategy;
        _context = context;
    }

    public async Task<GetTrainerFromUserAppResponse> Handle(GetTrainerFromUserAppRequest request, CancellationToken cancellationToken)
    {
        GetTrainerFromUserAppResponse resp = new();
        try
        {
            var user = await _userStrategy.GetAsync(request.userId);
            var linkedTrainer =
                await _context.Trainers.FirstOrDefaultAsync(trainer => trainer.Identity.UserId == request.userId, cancellationToken);
            if (linkedTrainer == null)
            {
                linkedTrainer = new Trainer(Name.Create(user.FirstName, user.LastName).Value,
                    TrainerIdentity.Create(user.UserId, Enumeration.FromDisplayName<ApplicationType>(user.ApplicationType)).Value, string.Empty,
                    Language.Create("EN").Value);
                await _context.Trainers.AddAsync(linkedTrainer, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
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
    public string userId { get; set; }
    public ApplicationType ApplicationType { get; set; }
}

public class GetTrainerFromUserAppResponse : ResponseBase
{
    public Trainer Trainer { get; set; }
}
