using MediatR;
using Smart.FA.Catalog.Application.SeedWork;
using Smart.FA.Catalog.Core.Domain.Authorization;
using Smart.FA.Catalog.Core.Domain.User.Enumerations;
using Smart.FA.Catalog.Core.Domain.ValueObjects;
using Smart.FA.Catalog.Infrastructure.Persistence;
using Smart.FA.Catalog.Shared.Domain.Enumerations.Common;

namespace Smart.FA.Catalog.Application.UseCases.Commands;

public class BlackListUserCommand : IRequestHandler<BlackListUserRequest, BlackListUserResponse>
{
    private readonly CatalogContext _catalogContext;

    public BlackListUserCommand(CatalogContext catalogContext)
    {
        _catalogContext = catalogContext;
    }

    public async Task<BlackListUserResponse> Handle(BlackListUserRequest request, CancellationToken cancellationToken)
    {
        BlackListUserResponse response = new();
        var blackListedUserIdentity = TrainerIdentity.Create(request.UserId, Enumeration<ApplicationType>.FromValue(request.ApplicationTypeId));
        var blackListedUser =  new BlackListedUser(blackListedUserIdentity.Value);
        _catalogContext.BlackListedUsers.Add(blackListedUser);
        await _catalogContext.SaveChangesAsync(cancellationToken);
        response.SetSuccess();

        return response;
    }
}

public class BlackListUserRequest : IRequest<BlackListUserResponse>
{
    public string UserId { get; set; }
    public int ApplicationTypeId { get; set; }
}

public class BlackListUserResponse : ResponseBase
{
}
