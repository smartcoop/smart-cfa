// Application

using Core.Exceptions;
using Core.SeedWork;

namespace Application.SeedWork;

/// <summary>
/// Base response for Mediatr handlers
/// </summary>
public class ResponseBase
{
    private readonly List<ApplicationError> _errors;
    public bool IsSuccess { get; private set; }
    public IReadOnlyCollection<ApplicationError> Errors => _errors;

    /// <summary>
    /// Creates a new response base
    /// By default, it contains no errors and is set to success = false
    /// </summary>
    /// <param name="success"></param>
    protected ResponseBase()
    {
        _errors = new List<ApplicationError>();
    }

    /// <summary>
    /// Adds a string error to the response
    /// </summary>
    /// <param name="message">The error message</param>
    public void AddError(string code, string message)
    {
        IsSuccess = false;
        _errors.Add(new ApplicationError(code, message));
    }

    /// <summary>
    /// Adds an error object to the response
    /// </summary>
    /// <param name="message">The error message</param>
    public void AddError(Error error)
    {
        IsSuccess = false;
        _errors.Add(new ApplicationError(error.Code, error.Message));
    }

    /// <summary>
    /// Adds string error to the response
    /// </summary>
    /// <param name="exception">The exception</param>
    public void AddError(DomainException exception)
    {
        AddError(exception.Error);
    }

    /// <summary>
    /// Removes all error messages and sets success to true
    /// </summary>
    public void SetSuccess()
    {
        IsSuccess = true;
        _errors.Clear();
    }

    /// <summary>
    /// Check if response contains any errors
    /// </summary>
    /// <returns>True if more than 0 errors</returns>
    public bool HasErrors()
    {
        return _errors.Count > 0;
    }
}

public class ApplicationError
{
    public string Code { get; set; }
    public string Message { get; set; }

    public ApplicationError(string code, string message)
    {
        Code = code;
        Message = message;
    }
}
