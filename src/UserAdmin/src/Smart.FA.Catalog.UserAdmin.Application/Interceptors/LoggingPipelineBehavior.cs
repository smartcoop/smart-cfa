using System.Text.Json;
using System.Text.Json.Serialization;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Smart.FA.Catalog.UserAdmin.Application.Models.Options;
using Smart.FA.Catalog.UserAdmin.Application.SeedWork;
using Smart.FA.Catalog.UserAdmin.Application.SeedWork.Json.Converters;
using Smart.FA.Catalog.UserAdmin.Domain.Exceptions;
using Smart.FA.Catalog.UserAdmin.Domain.Extensions;
using Smart.FA.Catalog.UserAdmin.Domain.LogEvents;
using Smart.FA.Catalog.UserAdmin.Domain.SeedWork;

namespace Smart.FA.Catalog.UserAdmin.Application.Interceptors;

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
        _mediatROptions = mediatROptions.Value ?? throw new ArgumentException($"{nameof(mediatROptions)} couldn't be bound");
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        if (_mediatROptions.LogRequests)
        {
            LogMediatRRequest(request);
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
            _logger.LogError(LogEventIds.ErrorEventId, exception, $"An exception occurred while handling `{SerializeRequest(request)}`");
        }

        return response;
    }

    private string SerializeRequest(TRequest request)
    {
        // ReferenceHandler.Preserve prevents circular references when serializing.
        // Ses https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-preserve-references?pivots=dotnet-6-0.
        var jsonSerializerOptions = new JsonSerializerOptions()
        {
            WriteIndented    = true,
            ReferenceHandler = ReferenceHandler.Preserve,
            Converters =
            {
                new TrainerJsonConverter(),
                new TrainingJsonConverter(),
                new UserChartRevisionJsonConverter(),
                new SuperUserJsonConverter(),
                new TrainerAssignmentJsonConverter()
            }
        };

        return request.ToJson(jsonSerializerOptions);
    }

    private void LogMediatRRequest(TRequest request)
    {
        _logger.LogInformation("MediatR request: {requestName} - `{request}`", request.GetType().Name, SerializeRequest(request));
    }
}
