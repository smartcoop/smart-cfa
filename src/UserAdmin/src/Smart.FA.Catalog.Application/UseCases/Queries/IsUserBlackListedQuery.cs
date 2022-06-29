using MediatR;
using Microsoft.EntityFrameworkCore;
using Smart.FA.Catalog.Application.SeedWork;
using Smart.FA.Catalog.Core.Domain.User.Enumerations;
using Smart.FA.Catalog.Infrastructure.Persistence;
using Smart.FA.Catalog.Shared.Domain.Enumerations.Common;

namespace Smart.FA.Catalog.Application.UseCases.Queries;

public class IsUserBlackListedQuery : IRequestHandler<IsUserBlackListedRequest, IsUserBlackListedResponse>
{
    private readonly CatalogContext _catalogContext;

    public IsUserBlackListedQuery(CatalogContext catalogContext)
    {
        _catalogContext = catalogContext;
    }

    public async Task<IsUserBlackListedResponse> Handle(IsUserBlackListedRequest request, CancellationToken cancellationToken)
    {
        IsUserBlackListedResponse response = new();
        var applicationType = ApplicationType.FromName(request.ApplicationType);
        var blackListedUser = await _catalogContext.BlackListedUsers.FirstOrDefaultAsync(blacklistedUser => blacklistedUser.UserId == request.UserId && blacklistedUser.ApplicationTypeId == applicationType, cancellationToken);
        response.IsBlackListed = blackListedUser is not null;
        response.SetSuccess();

        return response;
    }
}

public class IsUserBlackListedRequest : IRequest<IsUserBlackListedResponse>
{
    public string UserId { get; set; }
    public string ApplicationType { get; set; }
}

public class IsUserBlackListedResponse : ResponseBase
{
    public bool IsBlackListed { get; set; }
}
