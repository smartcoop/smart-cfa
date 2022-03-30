using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Smart.FA.Catalog.Application.Models.Options;
using Smart.FA.Catalog.Application.SeedWork;
using Smart.FA.Catalog.Core.Exceptions;
using Smart.FA.Catalog.Core.Extensions;
using Smart.FA.Catalog.Core.LogEvents;
using Smart.FA.Catalog.Core.SeedWork;

namespace Smart.FA.Catalog.Application.Interceptors;

/// <summary>
/// Global logger applied on all handlers.
/// It will format potential exception with the Error template when pertinent and set the response to a successful state, otherwise.
/// </summary>
/// <typeparam name="TRequest">The input object of the handler, it needs to inherit from IMediatr IRequest class</typeparam>
/// <typeparam name="TResponse">the output object of the handler, it needs to inherit from ResponseBase and be instantiable</typeparam>
public class LoggingPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse> where TResponse : ResponseBase, new()
{
    private readonly ILogger<Mediator> _logger;
    private readonly MediatROptions _mediatROptions;

    public LoggingPipelineBehavior
    (
        ILogger<Mediator> logger,
        IOptions<MediatROptions> mediatROptions)
    {
        _logger = logger;
        _mediatROptions = mediatROptions.Value ??
                          throw new ArgumentException($"{nameof(LoggingPipelineBehavior<TRequest, TResponse>)}: {nameof(mediatROptions)} not found", nameof(mediatROptions));
    }

    public async Task<TResponse> Handle
    (
        TRequest request,
        CancellationToken cancellationToken,
        RequestHandlerDelegate<TResponse> next
    )
    {
        if (_mediatROptions.LogRequests)
        {
            _logger.LogInformation("Incoming request: {requestName} - {request}", request.GetType().Name, request.ToJson());
        }
        TResponse response = new();
        try
        {
            response = await next();
        }
        catch (DomainException exception)
        {
            response.AddError(exception.Error);
            _logger.LogError(LogEventIds.ErrorEventId, exception.Error.Serialize());
        }
        catch (Exception exception)
        {
            response.AddError(Errors.General.UnexpectedHandlerError());
            _logger.LogError(LogEventIds.ErrorEventId, exception.ToString());
        }

        return response;
    }
}
