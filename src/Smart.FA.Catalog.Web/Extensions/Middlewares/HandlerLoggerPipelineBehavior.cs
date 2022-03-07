using Application.SeedWork;
using Core.Exceptions;
using Core.LogEvents;
using Core.SeedWork;
using MediatR;

namespace Web.Extensions.Middlewares;

/// <summary>
/// Global logger applied on all handlers.
/// It will format potential exception with the Error template when pertinent and set the response to a successful state, otherwise.
/// </summary>
/// <typeparam name="TRequest">The input object of the handler, it needs to inherit from IMediatr IRequest class</typeparam>
/// <typeparam name="TResponse">the output object of the handler, it needs to inherit from ResponseBase and be instantiable</typeparam>
public class HandlerLoggerPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse> where TResponse : ResponseBase, new()
{
    private readonly ILogger<HandlerLoggerPipelineBehavior<TRequest, TResponse>> _logger;

    public HandlerLoggerPipelineBehavior
    (
        ILogger<HandlerLoggerPipelineBehavior<TRequest, TResponse>> logger
    )
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle
    (
        TRequest request
        , CancellationToken cancellationToken,
        RequestHandlerDelegate<TResponse> next
    )
    {
        TResponse response = new();
        try
        {
            response = await next();
            response.SetSuccess();
        }
        catch (DomainException exception)
        {
            response.AddError(exception.Error);
            _logger.LogError(LogEventIds.ErrorEventId, exception.Error.Serialize());
        }
        catch (Exception exception)
        {
            response.AddError(Errors.General.UnexpectedHandlerError());
            _logger.LogError(LogEventIds.ErrorEventId, exception.Message);
        }

        return response;
    }
}
