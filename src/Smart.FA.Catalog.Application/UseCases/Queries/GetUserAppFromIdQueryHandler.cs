using Application.SeedWork;
using Core.Domain.Dto;
using Core.Domain.Enumerations;
using Core.Exceptions;
using Core.Extensions;
using Infrastructure.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Queries;

public class GetUserAppFromIdQueryHandler : IRequestHandler<GetUserAppFromIdRequest, GetUserAppFromIdResponse>
{
    private readonly ILogger<GetUserAppFromIdQueryHandler> _logger;
    private readonly UserStrategyResolver _userStrategyResolver;

    public GetUserAppFromIdQueryHandler
    (
        ILogger<GetUserAppFromIdQueryHandler> logger
        , UserStrategyResolver userStrategyResolver
    )
    {
        _logger = logger;
        _userStrategyResolver = userStrategyResolver;
    }

    public async Task<GetUserAppFromIdResponse> Handle(GetUserAppFromIdRequest query,
        CancellationToken cancellationToken)
    {
        GetUserAppFromIdResponse resp = new();

        var userStrategy = _userStrategyResolver.Resolve(query.ApplicationType!);
        resp.User = await userStrategy.GetAsync(query.UserId!);
        resp.SetSuccess();

        return resp;
    }
}

public class GetUserAppFromIdRequest : IRequest<GetUserAppFromIdResponse>
{
    public string UserId { get; init; } = null!;
    public ApplicationType ApplicationType { get; init; } = null!;
}

public class GetUserAppFromIdResponse : ResponseBase
{
    public UserDto User { get; set; } = null!;
}
